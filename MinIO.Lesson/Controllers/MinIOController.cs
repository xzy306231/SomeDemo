using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using MinIO.Lesson.Model;

namespace MinIO.Lesson.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinIOController : ControllerBase
    {
        private readonly IMinioClient client;

        public MinIOController()
        {
            this.client = client = new MinioClient()
                               .WithEndpoint("192.168.6.216:7000")
                               .WithCredentials("MkMjY6GXoFgAxpPYIUhi", "it35D6tHcEgYaHJGMPEDDV2V8pRqSXvvnivpfx8P")
                               .Build();
        }
        /// <summary>
        /// 列出所有桶
        /// </summary>
        /// <returns></returns>
        [Route("ListBucket")]
        [HttpGet]
        public async Task<object> ListBucket()
        {
            var list = await client.ListBucketsAsync();
            return list;
        }

        /// <summary>
        /// 创建桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [Route("bucketExists")]
        [HttpGet]
        public async Task BucketExists(string bucketName = "minio-dotnet-example-aaaaa")
        {

            try
            {
                Console.WriteLine("Running example for API: BucketExistsAsync");
                var args = new BucketExistsArgs()
                    .WithBucket(bucketName);
                var found = await client.BucketExistsAsync(args).ConfigureAwait(false);
                Console.WriteLine((found ? "Found" : "Couldn't find ") + "bucket " + bucketName);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [Route("getVersioning")]
        [HttpGet]
        public async Task GetVersioning(string bucketName = "my-bucket-name")
        {
            var args = new GetVersioningArgs().WithBucket(bucketName);

            try
            {
                Console.WriteLine("Running example for API: GetVersioning, ");
                var config = await client.GetVersioningAsync(args).ConfigureAwait(false);
                if (config is null)
                {
                    Console.WriteLine("Versioning Configuration not available for bucket " + bucketName);
                    Console.WriteLine();
                    return;
                }

                Console.WriteLine("Versioning Configuration Status " + config.Status + " for bucket " + bucketName);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }

        [Route("enableSuspendVersioning")]
        [HttpGet]
        public async Task EnableSuspendVersioning(string bucketName = "my-bucket-name")
        {
            try
            {
                Console.WriteLine("Running example for API: EnableSuspendVersioning, ");
                // First Enable the Versioning.
                var setArgs = new SetVersioningArgs()
                    .WithBucket(bucketName)
                    .WithVersioningEnabled();
                await client.SetVersioningAsync(setArgs).ConfigureAwait(false);
                Console.WriteLine("Versioning Enable operation called for bucket " + bucketName);
                // Next Suspend the Versioning.
                setArgs = setArgs.WithVersioningSuspended();
                await client.SetVersioningAsync(setArgs).ConfigureAwait(false);
                Console.WriteLine("Versioning Suspend operation called for bucket " + bucketName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }

        [Route("listBucketObject")]
        [HttpGet]
        public List<string> ListBucketObjects(string bucketName)
        {
            var res = new List<string>();
            var args = new ListObjectsArgs().WithBucket(bucketName).WithPrefix(null).WithRecursive(true);
            var list = client.ListObjectsAsync(args);
            list.Subscribe(a =>
            {
                Console.WriteLine(a.Key);
                res.Add(a.Key);
            },
                          ex =>
                          {
                              Console.WriteLine(ex.Message);
                          },
                          () =>
                          {
                              Console.WriteLine("{0}");
                              res = res;
                          });
            return res;
        }

        [Route("presignedGetObject")]
        [HttpGet]
        public async Task<string> PresignedGetObject(string bucketName = "my-bucket-name", string objectName = "my-object-name")
        {
            //var reqParams = new Dictionary<string, string>(StringComparer.Ordinal) { { "response-content-type", "application/json" }, 
            //                                                                         { "response-content-type", "application/vnd.ms-excel" } };

            var reqParams = new Dictionary<string, string>(StringComparer.Ordinal) { { "response-content-type", "image/png" } };

            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(20)
                .WithHeaders(reqParams);
            var presignedUrl = await client.PresignedGetObjectAsync(args).ConfigureAwait(false);
            return presignedUrl;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<UploadFileResult> Upload(FormFileCollection file)
        {
            CancellationToken cancellationToken = default;
            // Make a bucket on the server, if not already present.

            var uploadFileArgs = new UploadFileArgs()
            {
                BucketName = "richislandbucket1",
                FileName = file[0].FileName,
                FileStream = file[0].OpenReadStream(),
                ContentType = file[0].ContentType
            };

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
            var path = string.Join('/', uploadFileArgs.BucketName, uploadFileArgs.FileName);
            return new UploadFileResult { FilePath = path, Success = true };
        }

        [Route("download")]
        [HttpGet]
        public async Task<DownFileResult> DownLoad(string bucketName = "my-bucket-name", string objectName = "my-object-name", string fileName = "local-filename")
        {
            try
            {
                var stream = new MemoryStream();

                Console.WriteLine("Running example for API: GetObjectAsync");
                var args = new GetObjectArgs().WithBucket(bucketName).WithObject(objectName).WithFile($"D:\\minio\\data\\{objectName}");
                _ = await client.GetObjectAsync(args).ConfigureAwait(false);
                return new DownFileResult { Success = true };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
                return null;
            }
        }
    }
}
