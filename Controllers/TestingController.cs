using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using CliniqueBackend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestingApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestingController: ControllerBase
{
    private readonly AppDbContext _context;
    private FileUploader fileUploader;
    public TestingController(AppDbContext context, FileUploader fileUploader)
    {
        this._context = context;
        this.fileUploader = fileUploader;
    }
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

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] BlogPostDTO data)
    {
        var foundDepartment = await this._context
          .Department.FirstOrDefaultAsync(d => d.Id == data.DepartmentId);
        if (foundDepartment == null)
        {
            return NotFound();
        }
        var blogPost = new BlogPost { Content = data.Content, Author = data.Author };
        blogPost.Department = foundDepartment;
        blogPost.ExcerptTitle = data.ExcerptTitle;
        blogPost.ExcerptBody = data.ExcerptBody;
        
        var uri = await fileUploader.UploadFileAsync(data.ExcerptImage);
        blogPost.ExcerptImage = uri;
        this._context.BlogPost.Add(blogPost);
        await this._context.SaveChangesAsync();

        return NoContent();
    }
}