using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileProcessor.Services
{
    public class BlobManager : IBlobManager
    {
        private readonly BlobServiceClient _serviceClient;

        public BlobManager(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        public async Task<Stream> DownloadBlobAsync(string container, string blobName)
        {
            BlobContainerClient blobContainerClient = _serviceClient.GetBlobContainerClient(container);

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

            return blobDownloadInfo.Content;
        }

        public async Task<bool> UploadBlobAsync(string container, string blobName, Stream content)
        {
            BlobContainerClient blobContainerClient = _serviceClient.GetBlobContainerClient(container);

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content);

            return true;
        }
    }
}
