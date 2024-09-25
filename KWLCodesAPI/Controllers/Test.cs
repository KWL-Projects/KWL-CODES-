[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is working!");
    }
}