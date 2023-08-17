using FileProcessor.Starters;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace FileProcessorTests.Starters
{
    public class BlobTriggerStarterFacts
    {
        private readonly BlobTriggerStarter _sut;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IDurableOrchestrationClient> _durableOrchestrationClient;

        public BlobTriggerStarterFacts()
        {
            _sut = new BlobTriggerStarter();
            _logger = new Mock<ILogger>();
            _durableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
        }

        [Fact]
        public async Task Start_Should_CallTheOrchestrator()
        {
            byte[] bytes = Encoding.ASCII.GetBytes("test");

            var stream = new MemoryStream(bytes);

            await _sut.Start(stream, _durableOrchestrationClient.Object, "test.pdf", _logger.Object);

            _durableOrchestrationClient.Verify(x => x.StartNewAsync(It.IsAny<string>(), null, It.IsAny<string>()));
        }
    }
}
