using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using CliniqueBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BlogPostController: ControllerBase
{
    private readonly IBlogPostService blogPostService;
    private readonly AppDbContext _context;
    public BlogPostController(IBlogPostService blogPostService, AppDbContext context)
    {
        this.blogPostService = blogPostService;
        this._context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<BlogPostPagination>>> Get(int page = 1, int pageSize = 3)
    {
        var data = await this.blogPostService.FindAll(page, pageSize);
        return Ok(data);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogPost>> Get([FromRoute] int id)
    {
        var foundBlogPost = await this.blogPostService.FindOne(id);
        if (foundBlogPost == null)
        {
            return NotFound();
        }
        return Ok(foundBlogPost);
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
        await this.blogPostService.Create(foundDepartment, data);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] BlogPostDTO data)
    {

        var foundBlogPost = await this._context.BlogPost
           .FirstOrDefaultAsync(b => b.Id == id);
        if (foundBlogPost == null)
        {
            return NotFound();
        }
        var foundDepartment = await this._context
          .Department.FirstOrDefaultAsync(d => d.Id == data.DepartmentId);
        if (foundDepartment == null)
        {
            return NotFound();
        }
        await this.blogPostService
        .Update(foundDepartment, foundBlogPost, data);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {

        var foundBlogPost = await this._context.BlogPost
           .FirstOrDefaultAsync(b => b.Id == id);
        if (foundBlogPost == null)
        {
            return NotFound();
        }
        await this.blogPostService.Delete(foundBlogPost);
        return Ok();
    }
}