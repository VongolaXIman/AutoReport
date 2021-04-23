using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository
    {
        /// <summary>
        /// 取得全部的資料
        /// </summary>
        /// <typeparam name="K">Entity Type</typeparam>
        /// <returns></returns>
        Task<List<K>> GetAll<K>() where K : class;


        /// <summary>
        /// 取得多筆資料
        /// </summary>
        /// <typeparam name="K">Entity Type</typeparam>
        /// <param name="predicate">查詢條件</param>
        /// <returns></returns>
        Task<List<K>> Get<K>(Expression<Func<K, bool>> predicate) where K : class;


        Task<K> GetSingle<K>(Expression<Func<K, bool>> predicate) where K : class;


        Task<bool> IsExist<K>(Expression<Func<K, bool>> predicate) where K : class;


        Task<int> GetMax<K>(Expression<Func<K, bool>> predicate, Expression<Func<K, int>> selector) where K : class;


        void UpdateUnSave<K>(K entity) where K : class;

        void UpdateListUnSave<K>(List<K> entity) where K : class;

        void DeleteUnSave<K>(K entity) where K : class;

        void DeleteListUnSave<K>(List<K> entity) where K : class;

        void AddUnSave<K>(K entity) where K : class;

        void AddListUnSave<K>(List<K> entity) where K : class;

        Task<bool> SaveChangesAsync();
    }
}
