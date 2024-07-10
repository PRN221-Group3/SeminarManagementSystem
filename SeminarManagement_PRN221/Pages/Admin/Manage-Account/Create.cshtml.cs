using BusinessObject.Models;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    [Authorize(Roles = "Operator")]
    public class CreateModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventRepository _eventRepository;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public List<Role> Roles { get; set; } = new List<Role>();

        public Guid SponsorRoleId { get; private set; }

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public CreateModel(IUserRepository userRepository, IRoleRepository roleRepository, ISponsorRepository sponsorRepository, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _sponsorRepository = sponsorRepository;
            _eventRepository = eventRepository;
        }

        public async Task OnGetAsync(Guid? role, Guid? eventId)
        {
            Roles = await _roleRepository.GetAllRolesAsync();
            SponsorRoleId = await _roleRepository.GetSponsorRoleIdAsync(); // Assume this method gets the Sponsor Role Id

            if (role.HasValue && role.Value == SponsorRoleId)
            {
                UserDto.RoleId = SponsorRoleId;
            }

            ViewData["EventId"] = eventId;
        }

        public async Task<IActionResult> OnPostAsync(Guid? eventId)
        {
            Roles = await _roleRepository.GetAllRolesAsync();
            SponsorRoleId = await _roleRepository.GetSponsorRoleIdAsync(); // Assume this method gets the Sponsor Role Id

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please provide all the required fields.";
                return Page();
            }

            if (await _userRepository.IsEmailTakenAsync(UserDto.Email))
            {
                ModelState.AddModelError("UserDto.Email", "This email is already taken.");
                ErrorMessage = "This email is already taken.";
                return Page();
            }
            if (await _userRepository.IsUsernameTakenAsync(UserDto.Username))
            {
                ModelState.AddModelError("UserDto.Username", "This username is already taken.");
                ErrorMessage = "This username is already taken.";
                return Page();
            }

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = UserDto.FirstName,
                LastName = UserDto.LastName,
                Email = UserDto.Email,
                PhoneNumber = UserDto.PhoneNumber,
                Username = UserDto.Username,
                Password = UserDto.Password,
                RoleId = UserDto.RoleId,
                CreatedDate = DateTime.Now,
                IsActivated = true,
                IsDeleted = false,
            };

            GenerateQRCode(newUser);

            if (UserDto.RoleId == SponsorRoleId)
            {
                newUser.Sponsor = new Sponsor
                {
                    SponsorId = newUser.UserId,
                    SponsorName = UserDto.SponsorName,
                    SponsorType = UserDto.SponsorType,
                    IsDeleted = false
                };
            }

            await _userRepository.AddAsync(newUser);

            SuccessMessage = "User created successfully";

            if (UserDto.RoleId == SponsorRoleId)
            {
                return RedirectToPage("/Admin/Manage-Event/Sponsor", new { EventId = eventId });
            }

            return RedirectToPage("/Admin/Manage-Account/Manage");
        }

        private void GenerateQRCode(User user)
        {
            // QR Generator
            var qrGen = new QRCodeGenerator();
            var info = qrGen.CreateQrCode(
                $"Full Name: {user.FirstName} {user.LastName}, Email: {user.Email}",
                QRCodeGenerator.ECCLevel.Q
            );
            using var qrCode = new PngByteQRCode(info);
            var qrCodeImage = qrCode.GetGraphic(20);

            user.QrCode = Convert.ToBase64String(qrCodeImage);
        }
    }
}
