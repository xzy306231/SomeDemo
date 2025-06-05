using Microsoft.AspNetCore.Http;
using MinIoDemo.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MinIoDemo.IService
{
    public interface IMinIOService
    {
        Task<object> ListBucket();
        Task<object> CreateBucket(string buctetName);
        Task<object> DeleteBucket(string bucketName);
        object ListBucketObjects(string bucketName);
        Task<object> DeleteObject(string bucketName, string objectName);
        Task<UploadFileResult> Upload(UploadFileArgs uploadFileArgs, CancellationToken cancellationToken = default);
        Task<DownFileResult> Download(DownloadFileArgs downloadFileArgs, CancellationToken cancellationToken = default);
    }
}
