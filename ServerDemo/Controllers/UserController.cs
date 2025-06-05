using Microsoft.AspNetCore.Mvc;
using ServerDemo.Model;
using System.Threading.Tasks;

namespace ServerDemo.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController
    {
        public UserController()
        {

        }
        [HttpGet]
        [Route("users/{id}")]
        public User Get(string id)
        {
            return new User { Name = id };
        }
        [HttpPost("users")]
        public User Post(User user)
        {
            user.DateTime = System.DateTime.Now;
            return user;
        }
    }
}
