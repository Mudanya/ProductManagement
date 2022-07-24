using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UltimateApi.ActionFilters;
using UltimateApi.ModelBinder;

namespace UltimateApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompaniesController(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet,Authorize(Roles = "Manager")]
        [ResponseCache(CacheProfileName = "120SecondDuration")]
        public async Task<IActionResult> GetCompanies()
        {
            var _companies = await _repositoryManager.Company.GetAllCompanies(trackChanges: false);
            var _companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(_companies);
            return Ok(_companiesDto);

        }
        [HttpGet("{id}", Name = "CompanyById")]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _repositoryManager.Company.GetCompany(id, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"An error while getting company with id {id}");
                return NotFound();
            }
            var companyDto = _mapper.Map<CompanyDTO>(company);
            return Ok(companyDto);
        }
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogInfo("Null ids");
                return NotFound();
            }
            var companyCollection = await _repositoryManager.Company.GetByIds(ids, trackChanges: false);
            if (ids.Count() != companyCollection.Count())
            {
                _logger.LogInfo("Ids Mismatch with results!");
                return NotFound();
            }
            var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyCollection);
            return Ok(companyCollectionReturn);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repositoryManager.Company.CreateCompany(companyEntity);
            await _repositoryManager.Save();

            var companyDto = _mapper.Map<CompanyDTO>(companyEntity);
            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);

        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyCreateDto> companyCreateDtos)
        {
            if (companyCreateDtos == null)
            {
                _logger.LogInfo("Received empty list of companies");
                return BadRequest("Company collection is null");
            }
            var companyCollectionEntities = _mapper.Map<IEnumerable<Company>>(companyCreateDtos);
            foreach (var comp in companyCollectionEntities)
            {
                _repositoryManager.Company.CreateCompany(comp);
            }
            await _repositoryManager.Save();
            var companyCollectionDtos = _mapper.Map<IEnumerable<CompanyDTO>>(companyCollectionEntities);
            var ids = string.Join(",", companyCollectionDtos.Select(x => x.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionDtos);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var company = HttpContext.Items["company"] as Company;
            _repositoryManager.Company.DeleteCompany(company);
            await _repositoryManager.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        [ServiceFilter(typeof(ValidateCompanyExistAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateDto company)
        {
            var companyEntity = HttpContext.Items["company"] as Company;
            _mapper.Map(company, companyEntity);
            await _repositoryManager.Save();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
