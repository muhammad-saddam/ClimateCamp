using Azure.Storage.Blobs;
using Mobile.Combustion.Calculation.Models;
using System;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services.Common
{
    public class BlobFileService : IBlobFileService
    {
        //todo
        private string connectionString = Environment.GetEnvironmentVariable("BlobConnectionString");
        public async Task<System.IO.Stream> GetFileStream(ReadFileModel readFileModel)
        {
            try
            {
                BlobServiceClient serviceClient = new BlobServiceClient(connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(readFileModel.BlobContainerName);
                containerClient.CreateIfNotExists();
                BlobClient blobClient = containerClient.GetBlobClient($"{readFileModel.FileName}");
                return await blobClient.OpenReadAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
