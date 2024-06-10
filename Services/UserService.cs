using BusinessObject.Models;
using Repositories;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> AuthenticateUser(string usernameOrEmail, string password)
        {
            var user = await _userRepository.GetUserByUsernameOrEmail(usernameOrEmail);

            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<Role> GetRoleById(Guid? id)
        {
            return await _roleRepository.GetRoleById(id);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _roleRepository.GetRoleByName(roleName);
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash;
        }
        public async Task<string> GetRoleNameById(Guid? roleId)
        {
            if (roleId == null)
                return string.Empty;

            var role = await _roleRepository.GetRoleById(roleId.Value);
            return role?.RoleName ?? string.Empty;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _userRepository.GetUserByEmail(email) != null;
        }

    }
}
