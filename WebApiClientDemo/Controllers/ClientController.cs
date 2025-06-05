using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using WebApiClientDemo.IService;
using WebApiClientDemo.Model;

namespace WebApiClientDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IUserApi userApi;

        public ClientController(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        [Route("get")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<User> Get(int i)
        {
            var user = await userApi.GetAsync(i.ToString());
            return user;
        }

        [Route("api/users")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<User> PostAsync([JsonContent] User user)
        {
            var res = await userApi.PostAsync(user);
            return res;
        }
    }
}
