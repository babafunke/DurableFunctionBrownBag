using FileProcessor.Services;
using FileProcessor.Statics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileProcessor.Activities
{
    public class SaveFileForReleaseActivity
    {
        private readonly IBlobManager _blobManager;

        public SaveFileForReleaseActivity(IBlobManager blobManager)
        {
            _blobManager = blobManager ?? throw new ArgumentNullException(nameof(blobManager));
        }

        [FunctionName(StringConstants.SaveFileForReleaseActivity)]
        public async Task<bool> SaveFile([ActivityTrigger] string blobName, ILogger log)
        {
            log.LogInformation($"Saving file {blobName} for release");

            var content = await _blobManager.DownloadBlobAsync(StringConstants.BlobContainerName, $"prerelease/{blobName}");

            await _blobManager.UploadBlobAsync(StringConstants.BlobContainerName, $"release/{blobName}", content);

            return true;
        }
    }
}
