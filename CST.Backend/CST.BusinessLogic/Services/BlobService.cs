using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CST.Common.Repositories;
using Microsoft.Extensions.Configuration;

namespace CST.BusinessLogic.Services
{
    public class BlobService : IBlobService
    {
        private readonly Lazy<BlobContainerClient> _containerClient;

        public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            var containerName = configuration.GetValue<string>("AzureBlobStorageContainer");
            _containerClient = new Lazy<BlobContainerClient>(blobServiceClient.GetBlobContainerClient(containerName));
        }

        public async Task<Uri> UploadBlobAsync(string fileName, byte[] content, string contentType)
        {
            var blobClient = _containerClient.Value.GetBlobClient(fileName);

            await using var memoryStream = new MemoryStream(content);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = contentType });

            return blobClient.Uri;
        }
    }
}