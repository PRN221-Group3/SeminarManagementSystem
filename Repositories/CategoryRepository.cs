using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryRepository(CategoryDAO categoryDAO) : base(categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }
    }
}
