using FileProcessor.Activities;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileProcessorTests.Activities
{
    public class ValidateFileFormatActivityFacts
    {
        private readonly ValidateFileFormatActivity _sut;
        private readonly Mock<ILogger> _logger;

        public ValidateFileFormatActivityFacts()
        {
            _logger = new Mock<ILogger>();
            _sut = new ValidateFileFormatActivity();
            Environment.SetEnvironmentVariable("AcceptedFileFormats", ".pdf,.txt,.csv");
        }

        [Fact]
        public void Validate_Should_Return_True()
        {
            var result = _sut.Validate("text.pdf", _logger.Object);

            Assert.True(result);
        }

        [Fact]
        public void Validate_Should_Return_False()
        {
            var result = _sut.Validate("text.png", _logger.Object);

            Assert.False(result);
        }
    }
}
