using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ITemplateDbContext _context;

        public GenericRepository(ITemplateDbContext context)
        {
            _context = context;
        }

        public async Task<List<K>> GetAll<K>() where K : class
        {
            return await this._context.Set<K>().AsQueryable().ToListAsync();
        }

        public async Task<int> GetMax<K>(Expression<Func<K, bool>> predicate, Expression<Func<K, int>> selector) where K : class
        {
            await this._context.Set<K>().Where(predicate).MaxAsync(selector);
            return 0;
        }

        public async Task<List<K>> Get<K>(Expression<Func<K, bool>> predicate) where K : class
        {
            return await this._context.Set<K>().Where(predicate).AsQueryable().ToListAsync();
        }

        public Task<K> GetSingle<K>(Expression<Func<K, bool>> predicate) where K : class
        {
            return this._context.Set<K>().FirstOrDefaultAsync(predicate);
        }

        public Task<bool> IsExist<K>(Expression<Func<K, bool>> predicate) where K : class
        {
            return this._context.Set<K>().AnyAsync(predicate);
        }

        public void UpdateUnSave<K>(K entity) where K : class
        {
            try
            {
                this._context.Set<K>().Update(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void UpdateListUnSave<K>(List<K> entity) where K : class
        {
            try
            {
                this._context.Set<K>().UpdateRange(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void DeleteUnSave<K>(K entity) where K : class
        {
            try
            {
                this._context.Set<K>().Remove(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void DeleteListUnSave<K>(List<K> entity) where K : class
        {
            try
            {
                this._context.Set<K>().RemoveRange(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void AddUnSave<K>(K entity) where K : class
        {
            try
            {
                this._context.Set<K>().Add(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void AddListUnSave<K>(List<K> entity) where K : class
        {
            try
            {
                this._context.Set<K>().AddRange(entity);
            }
            catch
            {
                throw new ArgumentNullException("entity");
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                string error = "Message:" + ex.Message + "  StackTrace:" + ex.StackTrace;
                return false;
            }
        }
    }
}
