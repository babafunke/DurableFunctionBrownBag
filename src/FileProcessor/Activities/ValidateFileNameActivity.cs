using FileProcessor.Statics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace FileProcessor.Activities
{
    public class ValidateFileNameActivity
    {
        [FunctionName(StringConstants.ValidateFileNameActivity)]
        public bool Validate([ActivityTrigger] string blobName, ILogger log)
        {
            log.LogInformation($"Validating file name for {blobName}");

            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(blobName);

            if(fileNameWithoutExt.Length < 7)
            {
                throw new ArgumentOutOfRangeException("File name needs to be at least 7 characters");
            }

            var suffix = fileNameWithoutExt.Substring(fileNameWithoutExt.Length - 6);

            return int.TryParse(suffix, out int output);
        }
    }
}
