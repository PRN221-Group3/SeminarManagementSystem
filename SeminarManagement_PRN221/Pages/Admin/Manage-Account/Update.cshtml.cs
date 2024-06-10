using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class UpdateModel : PageModel
    {
        private readonly SeminarManagementDbContext _context;
        private readonly RoleRepository _roleRepository;
        private readonly UserRepository _userRepository;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public User User { get; set; } = new User();

        public List<Role> Roles { get; set; } = new List<Role>();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public UpdateModel(SeminarManagementDbContext context, RoleRepository roleRepository, UserRepository userRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

       public async Task<IActionResult> OnGetAsync(Guid? id)
{
    if (id == null)
    {
        ErrorMessage = "1";
                return Page();
    }

    using (var context = new SeminarManagementDbContext())
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
                    ErrorMessage = "2";
                    return Page();
                }

        UserDto.FirstName = user.FirstName;
        UserDto.LastName = user.LastName;
        UserDto.Email = user.Email;
        UserDto.PhoneNumber = user.PhoneNumber;
        UserDto.Username = user.Username;
        UserDto.Password = user.Password;
        UserDto.RoleId = user.RoleId;
        UserDto.QrCode = user.QrCode;

        Roles = await _roleRepository.GetAllRolesAsync();
        User = user;

        return Page();
    }
}

public async Task<IActionResult> OnPostAsync(Guid? id)
{
    if (id == null)
    {
                ErrorMessage = "3";
                return Page();
            }

    using (var context = new SeminarManagementDbContext())
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
                    ErrorMessage = "Ngu";
            return Page();
        }

        if (!ModelState.IsValid)
        {
            ErrorMessage = "Invalid user data";
            return Page();
        }

        user.FirstName = UserDto.FirstName;
        user.LastName = UserDto.LastName;
        user.Email = UserDto.Email;
        user.PhoneNumber = UserDto.PhoneNumber;
        user.Username = UserDto.Username;
        user.Password = UserDto.Password;
        user.RoleId = UserDto.RoleId;
        user.QrCode = UserDto.QrCode;
        user.UpdatedDate = DateTime.Now;


        await context.SaveChangesAsync();

        User = user;
        SuccessMessage = "User updated successfully";
        return RedirectToPage("/Admin/Manage-Account/Manage");
    }
}
    }
}