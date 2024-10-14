using FDMS_API.Models.RequestModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(MailRequest mailRequest);
    }
}
