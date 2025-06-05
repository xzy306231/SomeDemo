using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MinIoDemo.IService;
using MinIoDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinIoDemo.Controllers
{
    [ApiController]
    [Route("minio")]
    public class MinIoController : ControllerBase
    {
        private readonly IMinIOService minIOService;
        private readonly IConfiguration configuration;

        public MinIoController( IMinIOService minIOService, IConfiguration configuration)
        {
            this.minIOService = minIOService;
            this.configuration = configuration;
        }
        /// <summary>
        /// 创建桶
        /// </summary>
        /// <param name="buctetName"></param>
        /// <returns></returns>
        [Route("createBucket")]
        [HttpGet]
        public async Task<object> CreateBucket(string buctetName)
        {
            return await minIOService.CreateBucket(buctetName);
        }
        /// <summary>
        /// 删除桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [Route("deleteBucket")]
        [HttpGet]
        public async Task<object> DeleteBucket(string bucketName)
        {
            return await minIOService.DeleteBucket(bucketName);
        }
        /// <summary>
        /// 列出所有桶
        /// </summary>
        /// <returns></returns>
        [Route("listBuckets")]
        [HttpGet]
        public Task<object> ListBucket()
        {
            return minIOService.ListBucket();
        }
        /// <summary>
        /// 列出桶中所有对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [Route("listBucketObjects")]
        [HttpGet]
        public object ListBucketObjects(string bucketName)
        {
            return minIOService.ListBucketObjects(bucketName);
        }
        /// <summary>
        /// 删除指定桶中的指定对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        [Route("deleteObject")]
        [HttpGet]
        public async Task<object> DeleteObject(string bucketName, string objectName)
        {
            return await minIOService.DeleteObject(bucketName, objectName);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        public async Task<UploadFileResult> UploadImg(FormFileCollection file)
        {
            return await minIOService.Upload(new UploadFileArgs
            {
                BucketName = configuration["Minio:BucketName"],
                FileName = file[0].FileName,
                FileStream = file[0].OpenReadStream(),
                ContentType = file[0].ContentType
            });
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="fileArgs"></param>
        /// <returns></returns>
        [Route("download")]
        [HttpPost]
        public async Task<DownFileResult> Download(DownloadFileArgs fileArgs)
        {
            return await minIOService.Download(fileArgs);
        }
    }
}
