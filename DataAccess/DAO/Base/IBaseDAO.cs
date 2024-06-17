using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO.Base
{
    public interface IBaseDAO<T>
        where T : class
    {
        List<T> GetAll();
        IQueryable<T> GetAllQueryable();
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        void Create(T entity);
        void Update(T entity);
        void Delete(Guid id);
    }
}
