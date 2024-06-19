using BusinessObject.Models;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class UpdateModel : PageModel
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public User User { get; set; } = new User();

        public List<Role> Roles { get; set; } = new List<Role>();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public UpdateModel(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                ErrorMessage = "ID null ";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            User = await _userRepository.GetByIdAsync(id.Value);
            if (User == null || User.IsDeleted == true)
            {
                ErrorMessage = "Account deleted can't update ";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }


            UserDto.FirstName = User.FirstName;
            UserDto.LastName = User.LastName;
            UserDto.Email = User.Email;
            UserDto.PhoneNumber = User.PhoneNumber;
            UserDto.Username = User.Username;
            UserDto.Password = User.Password;
            UserDto.RoleId = User.RoleId;
            UserDto.QrCode = User.QrCode;

            Roles = await _roleRepository.GetAllRolesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                ErrorMessage = "ID null ";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            User = await _userRepository.GetByIdAsync(id.Value);
            if (User == null || User.IsDeleted == true)
            {
                ErrorMessage = "Account deleted can't update ";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }


            if (!ModelState.IsValid)
            {
                Roles = await _roleRepository.GetAllRolesAsync(); // Re-populate roles on validation error
                ErrorMessage = "Invalid user data";
                return Page();
            }

            User.FirstName = UserDto.FirstName;
            User.LastName = UserDto.LastName;
            User.Email = UserDto.Email;
            User.PhoneNumber = UserDto.PhoneNumber;
            User.Username = UserDto.Username;
            User.Password = UserDto.Password;
            User.RoleId = UserDto.RoleId;
            User.QrCode = UserDto.QrCode;
            User.UpdatedDate = DateTime.Now;

            await _userRepository.UpdateAsync(User);

            SuccessMessage = "User updated successfully";
            return RedirectToPage("/Admin/Manage-Account/Manage");
        }
    }
}
