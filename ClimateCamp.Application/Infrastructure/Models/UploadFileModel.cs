using Microsoft.AspNetCore.Http;

namespace ClimateCamp.Infrastructure.Models
{
    public class UploadFileModel
    {
        public IFormFile File { get; set; }
        public string FileNameWithExtension { get; set; }
        public string BlobContainerName { get; set; }
        public string Path { get; set; }
    }
}
