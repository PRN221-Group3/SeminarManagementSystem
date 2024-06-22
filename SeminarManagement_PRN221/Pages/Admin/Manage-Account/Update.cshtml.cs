using BusinessObject.Models;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
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
        private readonly ISponsorRepository _sponsorRepository;

        [BindProperty]
        public UserDto UserDto { get; set; } = new UserDto();

        public User User { get; set; } = new User();

        public List<Role> Roles { get; set; } = new List<Role>();

        public Guid SponsorRoleId { get; private set; }

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public UpdateModel(IRoleRepository roleRepository, IUserRepository userRepository, ISponsorRepository sponsorRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _sponsorRepository = sponsorRepository;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                ErrorMessage = "ID null";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            User = await _userRepository.GetByIdAsync(id.Value);
            if (User == null || User.IsDeleted == true)
            {
                ErrorMessage = "Account deleted can't update";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            Roles = await _roleRepository.GetAllRolesAsync();
            SponsorRoleId = await _roleRepository.GetSponsorRoleIdAsync();

            UserDto.FirstName = User.FirstName;
            UserDto.LastName = User.LastName;
            UserDto.Email = User.Email;
            UserDto.PhoneNumber = User.PhoneNumber;
            UserDto.Username = User.Username;
            UserDto.Password = User.Password;
            UserDto.RoleId = User.RoleId;
            UserDto.QrCode = User.QrCode;

            if (User.RoleId == SponsorRoleId)
            {
                var sponsor = await _sponsorRepository.GetByIdAsync(User.UserId);
                if (sponsor != null)
                {
                    UserDto.SponsorName = sponsor.SponsorName;
                    UserDto.SponsorType = sponsor.SponsorType;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                ErrorMessage = "ID null";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            User = await _userRepository.GetByIdAsync(id.Value);
            if (User == null || User.IsDeleted == true)
            {
                ErrorMessage = "Account deleted can't update";
                return RedirectToPage("/Admin/Manage-Account/Manage");
            }

            if (!ModelState.IsValid)
            {
                Roles = await _roleRepository.GetAllRolesAsync(); // Re-populate roles on validation error
                SponsorRoleId = await _roleRepository.GetSponsorRoleIdAsync(); // Ensure SponsorRoleId is populated
                ErrorMessage = "Invalid user data";
                return Page();
            }

            bool shouldUpdateQrCode = false;
            if (User.FirstName != UserDto.FirstName || User.LastName != UserDto.LastName || User.Email != UserDto.Email)
            {
                shouldUpdateQrCode = true;
            }

            User.FirstName = UserDto.FirstName;
            User.LastName = UserDto.LastName;
            User.Email = UserDto.Email;
            User.PhoneNumber = UserDto.PhoneNumber;
            User.Username = UserDto.Username;
            User.Password = UserDto.Password;
            User.RoleId = UserDto.RoleId;
            User.UpdatedDate = DateTime.Now;

            if (shouldUpdateQrCode)
            {
                GenerateQRCode(User);
            }

            if (UserDto.RoleId == SponsorRoleId)
            {
                var sponsor = await _sponsorRepository.GetByIdAsync(User.UserId);
                if (sponsor == null)
                {
                    sponsor = new Sponsor
                    {
                        SponsorId = User.UserId,
                        SponsorName = UserDto.SponsorName,
                        SponsorType = UserDto.SponsorType,
                        IsDeleted = false
                    };
                    await _sponsorRepository.AddAsync(sponsor);
                }
                else
                {
                    sponsor.SponsorName = UserDto.SponsorName;
                    sponsor.SponsorType = UserDto.SponsorType;
                    await _sponsorRepository.UpdateAsync(sponsor);
                }
            }

            await _userRepository.UpdateAsync(User);

            SuccessMessage = "User updated successfully";
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
