using Microsoft.AspNetCore.Mvc;

namespace MinIO.Lesson.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinioCallbackController : ControllerBase
    {
        public MinioCallbackController()
        {

        }
        [Route("/path")]  //这里不能随便改，要与webhook配置的路径对应
        [HttpPost]
        public object CallBack([FromBody] object args)
        {
            return null;
        }
    }
}
