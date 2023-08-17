using FileProcessor.Statics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace FileProcessor.Activities
{
    public class ValidateFileFormatActivity
    {
        [FunctionName(StringConstants.ValidateFileFormatActivity)]
        public bool Validate([ActivityTrigger] string blobName, ILogger log)
        {
            log.LogInformation($"Validating file format for {blobName}");

            var acceptedExts = Environment.GetEnvironmentVariable("AcceptedFileFormats");

            var acceptedExtsList = acceptedExts.Split(',');

            var fileExt = Path.GetExtension(blobName);

            return acceptedExtsList.Contains(fileExt);
        }
    }
}
