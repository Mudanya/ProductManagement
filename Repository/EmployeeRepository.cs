using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {

        }
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(
                e => e.CompanyId.Equals(companyId), trackChanges)
                .OrderBy(e=>e.Name)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .ToListAsync();

            return PagedList<Employee>
                .ToPagedList(employees, 
                employeeParameters.PageNumber, 
                employeeParameters.PageSize);
        }
        public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
            await FindByCondition(
                e => e.CompanyId.Equals(companyId) 
                && e.Id.Equals(employeeId), trackChanges)
                .SingleOrDefaultAsync();

        public void CreateEmployee(Guid CompanyId, Employee employee)
        {
            employee.CompanyId = CompanyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee emp) => Delete(emp);

    }
}
