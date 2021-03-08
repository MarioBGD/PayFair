using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();

        Task<int> Add(T entity, bool withId = false);
        Task<int> AddRange(IEnumerable<T> entity);

        Task<int> Remove(int id);

        Task<int> Update(T entity);
    }
}
