using System.IO;
using System.Threading.Tasks;

namespace FileProcessor.Services
{
    public interface IBlobManager
    {
        Task<Stream> DownloadBlobAsync(string container, string blobName);
        Task<bool> UploadBlobAsync(string container, string blobName, Stream content);
    }
}
