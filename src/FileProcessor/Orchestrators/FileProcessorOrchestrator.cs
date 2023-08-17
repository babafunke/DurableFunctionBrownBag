using FileProcessor.Statics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileProcessor.Orchestrators
{
    public class FileProcessorOrchestrator
    {
        [FunctionName(StringConstants.FileProcessorOrchestrator)]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            try
            {
                var blobName = context.GetInput<string>();

                var isFileFormatValid = await context.CallActivityAsync<bool>(StringConstants.ValidateFileFormatActivity, blobName);

                if (!isFileFormatValid)
                {
                    log.LogError("Step 1: File format is invalid");
                }

                log.LogInformation("Step 1: File format is valid");

                var isFileNameValid = await context.CallActivityAsync<bool>(StringConstants.ValidateFileNameActivity, blobName);

                if (!isFileNameValid)
                {
                    log.LogError("Step 2: File name is invalid");
                }

                log.LogInformation("Step 2: File name is valid");

                var isFileSaved = await context.CallActivityAsync<bool>(StringConstants.SaveFileForReleaseActivity, blobName);

                log.LogInformation("Step 3: File saved for release");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex.InnerException);
            }
        }
    }
}
