using FileProcessor.Activities;
using FileProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileProcessorTests.Activities
{
    public class SaveFileForReleaseActivityFacts
    {
        private SaveFileForReleaseActivity _sut;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IBlobManager> _blogManager;

        public SaveFileForReleaseActivityFacts()
        {
            _blogManager = new Mock<IBlobManager>();
            _logger = new Mock<ILogger>();
            _sut = new SaveFileForReleaseActivity(_blogManager.Object);

            _blogManager.Setup(b => b.DownloadBlobAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new MemoryStream());

            _blogManager.Setup(b => b.UploadBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()));
        }

        [Fact]
        public void Constructor_Should_ThrowException_If_Missing_BlobManager()
        {
            Assert.Throws<ArgumentNullException>(() => _sut = new SaveFileForReleaseActivity(null));
        }

        [Fact]
        public async Task SaveFile_Should_Return_Bool()
        {
            var result = await _sut.SaveFile("text20230506", _logger.Object);

            Assert.True(result);
        }

        [Fact]
        public async Task SaveFile_Should_Call_BlobManager_Service()
        {
            var result = await _sut.SaveFile("text20230506", _logger.Object);

            _blogManager.Verify(b => b.DownloadBlobAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _blogManager.Verify(b => b.UploadBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        }
    }
}
