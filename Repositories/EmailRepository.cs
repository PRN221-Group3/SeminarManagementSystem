using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Repositories.Interfaces;

public class EmailRepository : IEmailRepository
{
    private readonly string _myEmail = "seminarwebapp@gmail.com";
    private readonly string _myPassword = "mbyghvpzorxaihmp";

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage();
        message.From = new MailAddress(_myEmail);
        message.To.Add(new MailAddress(toEmail));
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(_myEmail, _myPassword),
            EnableSsl = true
        };

        try
        {
            await smtpClient.SendMailAsync(message);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
