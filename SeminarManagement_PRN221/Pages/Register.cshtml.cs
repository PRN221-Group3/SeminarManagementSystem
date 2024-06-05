using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using QRCoder;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace SeminarManagement_PRN221.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        [BindProperty]
        public UserDto User { get; set; }
        public string ConfirmPassword { get; set; }
        public RegisterModel(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public IActionResult OnPost()
        {
            UserGenToken();
            GenerateQRCode();

            var userToCreate = _mapper.Map<User>(User);
            userToCreate.UserId = Guid.NewGuid();

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
                return RedirectToPage("VerifyEmail");
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
            User.VerifyToken = KeyGenerator.GetUniqueKey(2);
            DateTime now = DateTime.Now;
            DateTime issueTokenDate = now.AddMinutes(5); // 5 minutes expiration

            User.IssueTokenDate = issueTokenDate;
        }

        private void GenerateQRCode()
        {
            // QR Generator
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData info = qrGen.CreateQrCode(
                $"Full Name: {User.FirstName} {User.LastName}, Email: {User.Email}",
                QRCodeGenerator.ECCLevel.Q
            );
            using var qrCode = new PngByteQRCode(info);
            var qrCodeImage = qrCode.GetGraphic(20);

            User.QrCode = $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
        }

        private void SendVerificationEmail(string email)
        {
            string myEmail = "seminarwebapp@gmail.com";
            string myPassword = "mbyghvpzorxaihmp";

            var message = new MailMessage();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = "[PRN221] Email Verification";

            message.Body = @"
                <html>
                <body>
                    <h2>Welcome to Seminar Web</h2>
                    <p>Thank you for registering. Please verify your email by clicking the button below:</p>
                    <a href='https://localhost:7271/Verify?email=" + email + "&token=" + User.VerifyToken + @"' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #ffffff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>Verify Email</a>
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
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}