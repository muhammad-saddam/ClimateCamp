using ClimateCamp.Infrastructure.Models;
using System.Threading.Tasks;

namespace ClimateCamp.Infrastructure.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(UploadFileModel uploadModel);
    }
}
