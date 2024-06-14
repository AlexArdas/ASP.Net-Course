using Azure.Storage.Blobs;
using CST.BusinessLogic.Services;
using CST.Tests.Common;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CST.BusinessLogic.Tests
{
    public class BlobServiceTests : AutoMockerTestsBase<BlobService>
    {
        private readonly Mock<BlobClient> _blobClient;

        private const string AzureBlobStorageContainerName = "test";

        public BlobServiceTests()
        {
            var configuration = GetMock<IConfiguration>();
            configuration.Setup(c => c.GetSection("AzureBlobStorageContainer").Value).Returns(AzureBlobStorageContainerName);

            var blobServiceClient = new Mock<BlobServiceClient>();

            _blobClient = new Mock<BlobClient>();
            _blobClient.Setup(i => i.BlobContainerName).Returns(AzureBlobStorageContainerName);

            var blobContainerClient = new Mock<BlobContainerClient>();
            blobContainerClient.Setup(cc => cc.GetBlobClient(It.IsAny<string>())).Returns(_blobClient.Object);

            blobServiceClient = new Mock<BlobServiceClient>();
            blobServiceClient.Setup(a => a.GetBlobContainerClient(AzureBlobStorageContainerName)).Returns(blobContainerClient.Object);

            Use(blobServiceClient.Object);
        }
    }
}
