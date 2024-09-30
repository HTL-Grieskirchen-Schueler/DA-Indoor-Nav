using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAServer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
  [HttpGet("TestGet")]
  public string TestGet()
  {
    this.Log();
    return "hallo";
  }

  public record TestRecord(string Message);
  [HttpPost("TestPost")]
  public void TestPost([FromBody]string record) => this.Log($"Message: {record}");
}
