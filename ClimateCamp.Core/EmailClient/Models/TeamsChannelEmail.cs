
namespace ClimateCamp.EmailClient.Models
{
    public class TeamsChannelEmail : EmailSenderModel
    {
        public string Organization { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string userPhone { get; set; }
    }
}
