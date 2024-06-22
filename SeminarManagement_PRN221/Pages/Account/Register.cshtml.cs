using System.Net;
using System.Net.Mail;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using QRCoder;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RegisterModel(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        [BindProperty] public UserDto User { get; set; }

        [BindProperty] public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (ConfirmPassword == null)
            {
                ModelState.AddModelError("ConfirmPassword", "Confirm Password is required");
                return Page();
            }
            if (!User.Password.Equals(ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Confirm Password Does not Match");
                return Page();
            }

            UserGenToken();
            GenerateQRCode();

            var role = await _roleRepository.GetRoleByName("User");
            var userToCreate = _mapper.Map<User>(User);
            userToCreate.UserId = Guid.NewGuid();
            userToCreate.Role = role;

            var existingUser = _userRepository.GetByEmail(User.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("User.Email", "Email already exists.");
                return Page();
            }

            try
            {
                _userRepository.Add(userToCreate);

                SendVerificationEmail(userToCreate.Email);
                return RedirectToPage("/VerifyEmail");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                return Page();
            }
        }

        private void UserGenToken()
        {
            User.CreatedDate = DateTime.Now;
            User.UpdatedDate = DateTime.Now;
            User.Username = User.Email.Split("@")[0];
            User.IsDeleted = false;
            User.IsActivated = false;
            User.VerifyToken = KeyGenerator.GetUniqueKey(16);
            var now = DateTime.Now;
            var issueTokenDate = now.AddMinutes(5); // 5 minutes expiration

            User.IssueTokenDate = issueTokenDate;
        }

        private void GenerateQRCode()
        {
            // QR Generator
            var qrGen = new QRCodeGenerator();
            var info = qrGen.CreateQrCode(
                $"Full Name: {User.FirstName} {User.LastName}, Email: {User.Email}",
                QRCodeGenerator.ECCLevel.Q
            );
            using var qrCode = new PngByteQRCode(info);
            var qrCodeImage = qrCode.GetGraphic(20);

            // Store only the Base64 string
            User.QrCode = Convert.ToBase64String(qrCodeImage);
        }

        private void SendVerificationEmail(string email)
        {
            var myEmail = "seminarwebapp@gmail.com";
            var myPassword = "mbyghvpzorxaihmp";

            var message = new MailMessage();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = "[PRN221] Email Verification";

            message.Body = @"
                <html>
                <body>
                    <h2>Welcome to Seminar Web</h2>
                    <p>Thank you for registering. Please verify your email by clicking the button below:</p>
                    <a href='https://localhost:7271/Verify?email=" + email + "&token=" + User.VerifyToken +
                       @"' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #ffffff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>Verify Email</a>
                    <br><br>
                    <p>If you did not request this email, please ignore it.</p>
                    <p>Best regards,<br>PRN221 Team</p>
                </body>
                </html>";

            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(myEmail, myPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
