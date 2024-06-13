using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public List<User> GetAll();
        public User GetById(Guid id);
        public void Add(User user);
        public User GetByEmail(string email);
        public void Update(User user);
        public Task<User> AuthenticateUser(string usernameOrEmail, string password);
        public Task<bool> IsEmailTakenAsync(string email);
        public Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        public Task<User> GetUserByEmail(string email);
        public Task<string> GetRoleNameById(Guid? roleId);
        public Task<Role> GetRoleById(Guid roleId);
        public Task<string> GetRoleNameById1(Guid? roleId);
        public Task<List<User>> GetAllUsersAsync();
        public Task<bool> IsUsernameTakenAsync(string username);
        public Task<User> GetUserByUsername(string username);
        public Task<List<User>> SearchUsersAsync(string searchQuery);
        public Task AddAsync(User user);
        public Task<User> GetByIdAsync(Guid id);
        public Task UpdateAsync(User user);

    }
}
