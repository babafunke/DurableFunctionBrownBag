using FileProcessor.Orchestrators;
using FileProcessor.Statics;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileProcessorTests.Orchestrators
{
    public class FileProcessorOrchestratorFacts
    {
        private readonly FileProcessorOrchestrator _sut;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IDurableOrchestrationContext> _context;
        private readonly string blobName = "text20230815";

        public FileProcessorOrchestratorFacts()
        {
            _logger = new Mock<ILogger>();
            _context = new Mock<IDurableOrchestrationContext>();
            _sut = new FileProcessorOrchestrator();
        }

        [Fact]
        public async Task RunOrchestrator_Should_Call_GetInput()
        {
            await _sut.RunOrchestrator(_context.Object, _logger.Object);

            _context.Verify(c => c.GetInput<string>(), Times.Once);
        }

        [Fact]
        public async Task RunOrchestrator_Should_ThrowException_If_Filename_IsEmptyOrNull()
        {
            try
            {
                _context.Setup(c => c.GetInput<string>()).Returns(string.Empty);

                await Assert.ThrowsAsync<Exception>(() => _sut.RunOrchestrator(_context.Object, _logger.Object));
            }
            catch { }
        }

        [Fact]
        public async Task RunOrchestrator_Should_Call_AllActivities()
        {
            _context.Setup(c => c.GetInput<string>()).Returns(blobName);

            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileFormatActivity, blobName))
                .ReturnsAsync(true);
            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName))
                .ReturnsAsync(true);
            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.SaveFileForReleaseActivity, blobName))
                .ReturnsAsync(true);

            await _sut.RunOrchestrator(_context.Object, _logger.Object);

            _context.Verify(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName), Times.Once);
            _context.Verify(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName), Times.Once);
            _context.Verify(c => c.CallActivityAsync<bool>(StringConstants.SaveFileForReleaseActivity, blobName), Times.Once);
        }

        [Fact]
        public async Task RunOrchestrator_Should_Not_Call_ValidateFileNameActivity_If_ValidateFormatActivity_IsFalse()
        {
            _context.Setup(c => c.GetInput<string>()).Returns(blobName);

            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileFormatActivity, blobName))
                .ReturnsAsync(false);

            await _sut.RunOrchestrator(_context.Object, _logger.Object);

            _context.Verify(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName), Times.Never);
        }

        [Fact]
        public async Task RunOrchestrator_Should_Not_Call_SaveFileForReleaseActivity_If_ValidateNameActivity_IsFalse()
        {
            _context.Setup(c => c.GetInput<string>()).Returns(blobName);

            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileFormatActivity, blobName))
                .ReturnsAsync(true);

            _context.Setup(c => c.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName))
                .ReturnsAsync(false);

            await _sut.RunOrchestrator(_context.Object, _logger.Object);

            _context.Verify(c => c.CallActivityAsync<bool>(StringConstants.SaveFileForReleaseActivity, blobName), Times.Never);
        }
    }
}
