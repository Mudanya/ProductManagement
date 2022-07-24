using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> :IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repository)
        {
            repositoryContext = repository;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? repositoryContext.Set<T>().AsNoTracking() 
            : repositoryContext.Set<T>();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges) =>
            !trackChanges ? repositoryContext.Set<T>().Where(predicate).AsNoTracking()
            : repositoryContext.Set<T>().Where(predicate);
        public void Create(T entity) => repositoryContext.Add(entity);
        public void Update(T entity) => repositoryContext.Update(entity);
        public void Delete(T entity) => repositoryContext.Remove(entity);
    }
}