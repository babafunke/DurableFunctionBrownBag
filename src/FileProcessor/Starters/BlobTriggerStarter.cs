using FileProcessor.Statics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace FileProcessor.Starters
{
    public class BlobTriggerStarter
    {
        [FunctionName(StringConstants.BlobTriggerStarter)]
        public async Task Start(
            [BlobTrigger($"{StringConstants.BlobContainerName}/prerelease/" + "{blobName}", Connection = "AzureWebJobsStorage")] Stream blob,
            [DurableClient] IDurableOrchestrationClient starter,
            string blobName,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(StringConstants.FileProcessorOrchestrator, null, blobName);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
        }
    }
}
