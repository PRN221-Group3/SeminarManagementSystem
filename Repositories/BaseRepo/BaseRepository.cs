using DataAccess.DAO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BaseRepo
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IBaseDAO<T> _dao;

        public BaseRepository(IBaseDAO<T> dao)
        {
            _dao = dao;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Task.Run(() => _dao.GetAll());
        }

        public T GetById(Guid id)
        {
            return _dao.GetById(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Task.Run(() => _dao.GetByIdAsync(id));
        }

        public async Task AddAsync(T entity)
        {
            await Task.Run(() => _dao.Create(entity));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => _dao.Update(entity));
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.Run(() => _dao.Delete(id));
        }
    }
}
