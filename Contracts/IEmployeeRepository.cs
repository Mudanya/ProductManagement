﻿using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParameters employeeParameters, bool trackChanges);
        Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        void CreateEmployee(Guid CompanyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
