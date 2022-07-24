using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UltimateApi.ActionFilters;

namespace UltimateApi.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> shaper;

        public EmployeesController(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            IDataShaper<EmployeeDto> _shaper
            )
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            shaper = _shaper;
        }
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetEmployees(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if(!employeeParameters.ValidAgeRange)
            {
                return BadRequest("Max age can't be less than min age.");
            }
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id {companyId} not found");
                return NotFound();
            }
            var employees = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(employees.MetaData));
            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(shaper.ShapeData(employeeDto, employeeParameters.Fields));
        }
        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid Id)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id {companyId} not Found!");
                return NotFound();
            }
            var employee = await _repository.Employee.GetEmployee(companyId, Id, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id {Id} Not found!");
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeCreateDto employee)
        {
          

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployee(companyId, employeeEntity);
            await _repository.Save();
            var employeeReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeReturn.Id }, employeeReturn);

        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForComapanyAttribute))]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
        {

            var employeeForCompany = HttpContext.Items["employee"] as Employee;
            if (employeeForCompany == null)
            {
                _logger.LogInfo($"Employee with id {id} not found!");
                return NotFound();
            }
            //
            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.Save();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        [ServiceFilter(typeof(ValidateEmployeeForComapanyAttribute))]
        public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeUpdateDto updateDto)
        {

            var employee = HttpContext.Items["employee"] as Employee;
            _mapper.Map(updateDto, employee);
            await _repository.Save();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForComapanyAttribute))]
        public async Task<IActionResult> UpdateEmployeePartially(Guid id, Guid companyId,
            [FromBody] JsonPatchDocument<EmployeeUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogInfo("PatchDoc Object was null");    
                return BadRequest();
            }

            var employeeEntity = HttpContext.Items["employee"] as Employee;
            var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
            patchDoc.ApplyTo(employeeToPatch, ModelState);
            TryValidateModel(employeeToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogInfo("Patch Validation error for EmployeeUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.Save();
            return NoContent();
        }

    }
}
