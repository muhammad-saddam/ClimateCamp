
namespace ClimateCamp.EmailClient.Models
{
    public class ActivateUserEmailDto : EmailSenderModel
    {
        public string ActivationLink { get; set; }
        public string SupportLink { get; set; }
    }
}
