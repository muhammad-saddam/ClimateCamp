
namespace ClimateCamp.EmailClient.Models
{
    public class InviteSupplierEmail : EmailSenderModel
    {
        public string PersonalMessage { get; set; }
        public string SupportLink { get; set; }
    }
}
