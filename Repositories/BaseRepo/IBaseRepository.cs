using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BaseRepo
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllQueryableAsync();
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        T GetById(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
