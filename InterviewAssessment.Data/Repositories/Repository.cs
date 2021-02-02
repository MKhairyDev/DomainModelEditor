using InterviewAssessment.Data.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InterviewAssessment.Data
{
   public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly EntityContext Context;
        public Repository(EntityContext context)
        {
            Context = context;
        }
        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await Context.Set<TEntity>().AddRangeAsync(entity);
        }
        public  void Remove(TEntity entities)
        {
             Context.Set<TEntity>().Remove(entities);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
             Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
