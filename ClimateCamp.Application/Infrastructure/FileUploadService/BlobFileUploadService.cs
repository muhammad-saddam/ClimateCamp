using Abp.Application.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ClimateCamp.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Infrastructure.FileUploadService
{
    /// <summary>
    ///generic  File Upload service 
    /// </summary>
    public class BlobFileUploadService : ApplicationService, IFileUploadService
    {
        private readonly IConfiguration _config;
        /// <summary>
        /// Azure Blob Upload Service
        /// </summary>
        /// <param name="config"></param>
        public BlobFileUploadService(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Upload Logo
        /// </summary>
        /// <param name="uploadModel"></param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(UploadFileModel uploadModel)
        {
            try
            {
                BlobServiceClient serviceClient = new BlobServiceClient(_config.GetValue<string>("App:BlobStorageConnectionString"));
                var container = serviceClient.GetBlobContainerClient(uploadModel.BlobContainerName);
                container.CreateIfNotExists();
                BlobClient blobClient = container.GetBlobClient(uploadModel.Path + "/" + uploadModel.FileNameWithExtension);
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                if (uploadModel.FileNameWithExtension != null && uploadModel.File != null)
                {
                    await blobClient.UploadAsync(uploadModel.File.OpenReadStream(), true);
                    return blobClient.Uri.AbsoluteUri;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("BlobFileUploadService :UploadFileAsync  " + ex.Message);
                throw ex;
            }
        }
    }
}
