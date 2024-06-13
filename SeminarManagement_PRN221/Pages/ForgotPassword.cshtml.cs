using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Net;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        private readonly IUserRepository _userRepository;
        public ForgotPasswordModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult OnGet()
        {
            return Page();
            
        }

        public IActionResult OnPost()
        {
            var token = GenerateToken(Email);
            TempData["Token"] = token;
            SendVerificationEmail(Email);
            return RedirectToPage("/VerifyToken");
        }
        private string GenerateToken(string email)
        {
            User user = _userRepository.GetByEmail(email);
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewData["msg"] = "Email invalid";
                Page();
            }

            if (user == null)
            {
                ViewData["msgInvalid"] = "Not found any account created by this email";
                Page();
            }

            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.AddMinutes(5).Ticks);
            byte[] _email = BitConverter.GetBytes(email != null);
            byte[] data = new Byte[_time.Length + _email.Length];

            Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            Buffer.BlockCopy(_email, 0, data, _time.Length, _email.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        private void SendVerificationEmail(string email)
        {
            string myEmail = "seminarwebapp@gmail.com";
            string myPassword = "mbyghvpzorxaihmp";
            string token = GenerateToken(email);
            var message = new MailMessage();
            message.From = new MailAddress(myEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = "[PRN221] Email Verification";

            message.Body = @"
                <html>
                <body>
                    <h2>Welcome to Seminar Web</h2>
                    <p>To reset password. Please verify your email by clicking the button below:</p>
                    <a href='https://localhost:7271/ResetPassword?email=" + email + "&token=" + token + @"' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #ffffff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>Verify Email</a>
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
