using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UltimateApi.ActionFilters
{
    public class ValidateEmployeeForComapanyAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        public ValidateEmployeeForComapanyAttribute(ILoggerManager _logger, IRepositoryManager _repository)
        {
            this._repository = _repository;
            this._logger = _logger;

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var companyId = (Guid)context.ActionArguments["companyId"];
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;

            var comapany = await _repository.Company.GetCompany(companyId, trackChanges:false);
            if(comapany == null)
            {
                _logger.LogInfo($"Company with id {companyId} not found");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];
            var employee = await _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if(employee == null)
            {
                _logger.LogInfo($"Employee with id {id} Not found in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);
                await next();
            }

        }
    }
}
