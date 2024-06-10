using BusinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class CreateModel : PageModel
    {
        private readonly SeminarManagementDbContext _context;
        private readonly RoleService _roleService;
        private readonly UserService _userService;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public List<Role> Roles { get; set; } = new List<Role>();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public CreateModel(SeminarManagementDbContext context, RoleService roleService, UserService userService)
        {
            _context = context;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleService.GetAllRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _userService.IsEmailTakenAsync(UserDto.Email))
            {
                ModelState.AddModelError("UserDto.Email", "This email is already taken.");
                ErrorMessage = "This email is already taken.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please provide all the required fields.";
                return Page();
            }

            User user = new User()
            {
                FirstName = UserDto.FirstName,
                LastName = UserDto.LastName,
                Email = UserDto.Email,
                PhoneNumber = UserDto.PhoneNumber,
                Username = UserDto.Username,
                Password = UserDto.Password,
                RoleId = UserDto.RoleId,
                QrCode = UserDto.QrCode,
                CreatedDate = DateTime.Now,
                IsActivated = true,
                IsDeleted = false,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SuccessMessage = "User created successfully";
            return RedirectToPage("/Admin/Manage-Account/Manage");
        }
    }
}
