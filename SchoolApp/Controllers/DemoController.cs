using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.SomeHelper;

namespace SchoolApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{message:alpha}")]
        public ActionResult Index(string message)
        {
            

            return Ok();
        }
    }
}
