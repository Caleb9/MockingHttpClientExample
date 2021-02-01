using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly HttpClientIsolationService _service;

        public HelloController(
            HttpClientIsolationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await _service.GetMessage();
        }
    }
}