using CliniqueBackend.Data;
using CliniqueBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestingController: ControllerBase
{
    private readonly AppDbContext _context;
    public TestingController(AppDbContext context) => this._context = context;
    [HttpGet]
    public ActionResult<Dictionary<string,string>> Get()
    {
        Dictionary<string,string> response = new Dictionary<string, string>();
        response.Add("firstName", "jonathan");
        response.Add("lastName", "nakahonda");
        return Ok(response);
    }

    [HttpGet]
    [Route("/api/[controller]/blogs")]
    public async Task<ActionResult<List<BlogPost>>> GetAll()
    {
        var blogPosts = await this._context.BlogPost.ToListAsync();
        return Ok(blogPosts);
    }
}