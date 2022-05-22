using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBucketList.Core.Interfaces.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IList<T>> Get();
        Task<T> Get(string id);
        Task<IList<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<int> InsertItem(T entity);
        Task<int> UpdateItem(T entity);
        Task<int> DeleteItem(T entity);
        Task<int> DeleteAllItems();
        Task<int> InsertAllItems(IEnumerable<T> list);
        Task<IList<T>> Query(string query, params object[] args);
        Task<int> Execute(string sqlText);
    }
}
