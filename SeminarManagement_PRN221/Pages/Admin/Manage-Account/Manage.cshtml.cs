using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class IndexModel : PageModel
    {
        private readonly UserRepository _userRepository;

        public IndexModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> Users { get; set; } = new List<User>();
        public Dictionary<Guid, string> RoleNames { get; set; } = new Dictionary<Guid, string>();

        public async Task OnGetAsync(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                Users = await _userRepository.GetAllUsersAsync();
            }
            else
            {
                Users = await _userRepository.SearchUsersAsync(searchQuery);
            }

            foreach (var user in Users)
            {
                if (user.RoleId.HasValue && !RoleNames.ContainsKey(user.RoleId.Value))
                {
                    var roleName = await _userRepository.GetRoleNameById1(user.RoleId.Value);
                    RoleNames[user.RoleId.Value] = roleName;
                }
            }
        }
    }
}
