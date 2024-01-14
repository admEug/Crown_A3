using App.Application.DTOs.Email;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}