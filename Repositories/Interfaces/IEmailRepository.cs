using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}
