using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        //public List<User> GetAll();
        //public User GetById(Guid id);
        //public void Add(User user);
        public User GetByEmail(string email);
        public void Update(User user);
    }
}
