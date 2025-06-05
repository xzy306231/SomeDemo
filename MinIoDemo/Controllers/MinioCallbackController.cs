//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Minio;
//using MinIoDemo.Model;
//using Newtonsoft.Json;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MinIoDemo.Controllers
//{
//    public class MinioCallbackController : ControllerBase
//    {
//        private static string _bucketName = string.Empty;//"demo";//默认桶

//        private readonly MinioClient _client;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<MinioCallbackController> _logger;

//        public MinioCallbackController(MinioClient client,
//            IConfiguration configuration,
//            ILogger<MinioCallbackController> logger)
//        {
//            _client = client;
//            _configuration = configuration;
//            _bucketName = configuration["MinIO:Bucket"];
//            _logger = logger;
//        }

//        [Route("/path")]  //这里不能随便改，要与webhook配置的路径对应
//        [HttpPost]
//        public async Task<IActionResult> CallBack([FromBody] object args)
//        {
//            try
//            {
//                string jsonStr = args.ToString();
//                var jsonObj = JsonConvert.DeserializeObject<Root>(jsonStr);
//                string bucketDirectory = _configuration["MinIO:BucketDirectory"];//文件保存目录
//                var pathAll = jsonObj.Key;//获取文件路径
//                var fileName = pathAll.Split('/').Last();

//                //新增
//                if (jsonObj.EventName.Contains("Put"))
//                {
//                    if (!System.IO.File.Exists(bucketDirectory))
//                    {
//                        DirectoryInfo directoryInfo = new DirectoryInfo(bucketDirectory);
//                        directoryInfo.Create();
//                    }

//                    var memoryStream = new MemoryStream();
//                    var path = jsonObj.Key.Substring(pathAll.IndexOf('/') + 1, pathAll.Length - pathAll.IndexOf('/') - 1);
//                    await _client.GetObjectAsync(_bucketName, path,
//                                    (stream) =>
//                                    {
//                                        stream.CopyTo(memoryStream);
//                                    });

//                    using FileStream targetFileStream = new FileStream($"{bucketDirectory}\\{fileName}", FileMode.OpenOrCreate);

//                    memoryStream.WriteTo(targetFileStream);
//                }
//                else if (jsonObj.EventName.Contains("Delete"))
//                {
//                    var path = $"{bucketDirectory}\\{fileName}";
//                    System.IO.File.Delete(path);
//                }


//                return Ok();
//            }
//            catch (System.Exception ex)
//            {
//                _logger.LogError($"ERROR:{ex}");
//            }

//            return null;
//        }

//    }
//}
