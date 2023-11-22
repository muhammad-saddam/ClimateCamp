using ClimateCamp.EmailClient.Models;
using System.Threading.Tasks;

namespace ClimateCamp.EmailClient.Services
{
    public interface IEmailClientSender
    {
        Task<string> SendEmail(EmailSenderModel emailModel, string EmailSenderServiceUrl);
    }
}
