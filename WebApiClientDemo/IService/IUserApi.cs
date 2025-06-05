using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using WebApiClientDemo.Model;

namespace WebApiClientDemo.IService
{
  //  [HttpHost("http://localhost:5000/")]
    public interface IUserApi
    {
        [HttpGet("api/users/{id}")]
        Task<User> GetAsync(string id);

        [HttpPost("api/users")]
        Task<User> PostAsync([JsonContent] User user);
    }
}
