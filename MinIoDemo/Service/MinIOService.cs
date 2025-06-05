using Minio;
using MinIoDemo.IService;
using System.IO;
using System.Threading.Tasks;
using MinIoDemo.Model;
using System.Threading;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;
using Minio.DataModel;
using System;

namespace MinIoDemo.Service
{
    public class MinIOService : IMinIOService
    {
        IMinioClient client;

        public MinIOService(IConfiguration configuration)
        {
            client = new MinioClient()
                               .WithEndpoint("192.168.6.216:7000")
                               .WithCredentials("MkMjY6GXoFgAxpPYIUhi", "it35D6tHcEgYaHJGMPEDDV2V8pRqSXvvnivpfx8P")
                               .Build();

        }
        public async Task<object> ListBucket()
        {
            var list = await client.ListBucketsAsync();
            return list;
        }
        public async Task<object> CreateBucket(string buctetName)
        {
            CancellationToken cancellationToken = default;
            var beArgs = new BucketExistsArgs().WithBucket(buctetName);
            bool found = await client.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(buctetName);
                await client.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
            }
            return null;
        }
        public async Task<object> DeleteBucket(string bucketName)
        {
            CancellationToken cancellationToken = default;
            var arg = new RemoveBucketArgs().WithBucket(bucketName);
            await client.RemoveBucketAsync(arg, cancellationToken);
            return null;
        }

        /// <summary>
        /// 列出桶中所有对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public object ListBucketObjects(string bucketName)
        {
            var args = new ListObjectsArgs().WithBucket(bucketName).WithPrefix(null).WithRecursive(true);
            var list = client.ListObjectsAsync(args);
            list.Subscribe(a => Console.WriteLine(a.Key),
                          ex => Console.WriteLine(ex.Message),
                          () => Console.WriteLine("{0}"));
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public async Task<object> DeleteObject(string bucketName, string objectName)
        {
            var arg = new RemoveObjectArgs().WithBucket(bucketName).WithObject(objectName);
            await client.RemoveObjectAsync(arg);
            return null;
        }
        public async Task<UploadFileResult> Upload(UploadFileArgs uploadFileArgs, CancellationToken cancellationToken = default)
        {
            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs().WithBucket(uploadFileArgs.BucketName);
            bool found = await client.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(uploadFileArgs.BucketName);
                await client.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
            }
            // Upload a file to bucket.
            var putObjectArgs = new PutObjectArgs().WithBucket(uploadFileArgs.BucketName).WithObject(uploadFileArgs.FileName)
                                                   .WithStreamData(uploadFileArgs.FileStream).WithObjectSize(uploadFileArgs.FileStream.Length)
                                                   .WithContentType(uploadFileArgs.ContentType);
            await client.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
            var path = BuildPath(uploadFileArgs.BucketName, uploadFileArgs.FileName);
            return new UploadFileResult { FilePath = path, Success = true };
        }
        public async Task<DownFileResult> Download(DownloadFileArgs downloadFileArgs, CancellationToken cancellationToken = default)
        {
            try
            {
                var stream = new MemoryStream();
                var args = downloadFileArgs.Path.Split("/");
                var getObjectArgs = new GetObjectArgs().WithBucket(args[0]).WithObject(args[1]).WithFile(args[1]);
                var response = await client.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);
                stream.Position = 0;
                return new DownFileResult { Stream = stream, Success = true, FileName = response.ObjectName, ContentType = response.ContentType };
            }
            catch (MinioException e)
            {
                return new DownFileResult { Success = false };
            }
        }
        private string BuildPath(string bucketName, string fileName)
        {
            return string.Join('/', bucketName, fileName);
        }


    }
}
