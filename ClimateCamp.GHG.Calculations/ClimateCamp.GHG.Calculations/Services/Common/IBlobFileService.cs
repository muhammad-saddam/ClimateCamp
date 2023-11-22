using Mobile.Combustion.Calculation.Models;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services.Common
{
    public interface IBlobFileService
    {
        //string ReadFile(ReadFileModel readFileModel);
        Task<System.IO.Stream> GetFileStream(ReadFileModel readFileModel);
    }
}
