using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UltimateApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager repository;

        public CompaniesV2Controller(IRepositoryManager repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await repository.Company.GetAllCompanies(trackChanges:false);
            return Ok(companies);
        }
    }
}
