﻿using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
        public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges) =>
            await
            FindAll(trackChanges).OrderBy(c => c.Name)
            .ToListAsync();
        public async Task<Company> GetCompany(Guid id, bool trackChanges) =>
            await FindByCondition(c => c.Id == id, trackChanges)
            .SingleOrDefaultAsync();
        public void CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void DeleteCompany(Company company) => Delete(company);
    }
}
