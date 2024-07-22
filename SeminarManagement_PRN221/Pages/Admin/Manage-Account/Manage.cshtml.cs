using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    [Authorize(Roles = "Operator")]
    public class ManageModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ManageModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> Users { get; set; } = new List<User>();
        public Dictionary<Guid, string> RoleNames { get; set; } = new Dictionary<Guid, string>();
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

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

        public async Task<IActionResult> OnPostDeleteAsync(Guid userId)
        {
            try
            {
                var userToDelete = await _userRepository.GetByIdAsync(userId);

                if (userToDelete == null)
                {
                    ErrorMessage = "User not found.";
                    await OnGetAsync(" ");
                    return Page();
                }

                userToDelete.IsDeleted = true;
                await _userRepository.UpdateAsync(userToDelete);

                SuccessMessage = "User deleted successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting user: {ex.Message}";
            }

            await OnGetAsync(" "); // Refresh the data after deletion
            return Page();
        }
    }
}
