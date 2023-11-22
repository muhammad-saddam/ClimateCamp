

namespace ClimateCamp.EmailClient.Models
{
    public abstract class EmailSenderModel
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public string LogoUrl { get; set; }
        public string Body { get; set; }
        public bool IsSelfServiceUser { get; set; }
        public string FirstName { get; set; }
    }
}
