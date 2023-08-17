using FileProcessor.Activities;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileProcessorTests.Activities
{
    public class ValidateFileNameActivityFacts
    {
        private readonly ValidateFileNameActivity _sut;
        private readonly Mock<ILogger> _logger;

        public ValidateFileNameActivityFacts()
        {
            _logger = new Mock<ILogger>();
            _sut = new ValidateFileNameActivity();
        }

        [Fact]
        public void Validate_Should_Return_True()
        {
            var result = _sut.Validate("text20230601.pdf", _logger.Object);

            Assert.True(result);
        }

        [Fact]
        public void Validate_Should_Return_False()
        {
            var result = _sut.Validate("text2023ab.pdf", _logger.Object);

            Assert.False(result);
        }

        [Fact]
        public void Validate_Should_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.Validate("text.png", _logger.Object));
        }
    }
}
