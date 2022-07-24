using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UltimateApi.ActionFilters
{
    public class ValidateCompanyExistAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateCompanyExistAttribute(IRepositoryManager _repository, ILoggerManager _logger)
        {
            this._repository = _repository;
            this._logger = _logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var company = await _repository.Company.GetCompany(id, trackChanges);

            if(company == null)
            {
                _logger.LogInfo($"Company with Id {id} doesnt exist in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
               context.HttpContext.Items.Add("company", company);
                await next();
            }
        }
    }
}
