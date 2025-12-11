using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestingController: ControllerBase
{
    [HttpGet]
    public ActionResult<Dictionary<string,string>> Get()
    {
        Dictionary<string,string> response = new Dictionary<string, string>();
        response.Add("firstName", "jonathan");
        response.Add("lastName", "nakahonda");
        return Ok(response);
    }
}