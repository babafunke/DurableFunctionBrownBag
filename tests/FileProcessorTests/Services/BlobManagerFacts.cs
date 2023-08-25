using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileProcessor.Services;
using Moq;

namespace FileProcessorTests.Services
{
    public class BlobManagerFacts
    {
        private BlobManager _sut;
        private readonly Mock<BlobServiceClient> _mockServiceClient;
        private readonly Mock<BlobContainerClient> _mockContainerClient;
        private readonly Mock<BlobClient> _mockClient;
        private readonly Mock<Response<BlobDownloadResult>> _mockDownloadResult;

        public BlobManagerFacts()
        {
            _mockServiceClient = new Mock<BlobServiceClient>();
            _mockContainerClient = new Mock<BlobContainerClient>();
            _mockClient = new Mock<BlobClient>();
            _mockDownloadResult = new Mock<Response<BlobDownloadResult>>();

            _sut = new BlobManager(_mockServiceClient.Object);

            _mockServiceClient.Setup(m => m.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_mockContainerClient.Object);

            _mockContainerClient.Setup(m => m.GetBlobClient(It.IsAny<string>()))
                .Returns(_mockClient.Object);

            byte[] data = new byte[1024];

            var binaryData = new BinaryData(data);

            var mockResult = BlobsModelFactory.BlobDownloadResult(binaryData);

            _mockDownloadResult.Setup(m => m.Value).Returns(mockResult);

            _mockClient.Setup(m => m.DownloadContentAsync())
                .ReturnsAsync(_mockDownloadResult.Object);

            _mockClient.Setup(m => m.UploadAsync(It.IsAny<string>()));
        }

        [Fact]
        public void Constructor_Should_Throw_Exception_If_BlobServiceClient_IsMissing()
        {
            Assert.Throws<ArgumentNullException>(() => _sut = new BlobManager(null));
        }

        [Fact]
        public async Task DownloadBlobAsync_Should_Return_Stream_Type()
        {
            var result = await _sut.DownloadBlobAsync(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsAssignableFrom<Stream>(result);
        }

        [Fact]
        public async Task DownloadBlobAsync_Should_Return_Bool_Type()
        {
            var result = await _sut.UploadBlobAsync(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<Stream>());

            Assert.True(result);
        }
    }
}
