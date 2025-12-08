using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Services;

public class BlogPostService: IBlogPostService
{
    private readonly AppDbContext _context;
    private readonly FileUploader fileUploader;
    public BlogPostService(AppDbContext context, FileUploader fileUploader)
    {
        this._context = context;
        this.fileUploader = fileUploader;
    }

    public async Task<BlogPostPagination> FindAll(int page = 1, int pageSize = 3)
    {
        var totalCount = await this._context.BlogPost.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        var blogPosts = await this._context
            .BlogPost
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(b => b.Department)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        var data = new BlogPostPagination
        {
            Data = blogPosts,
            TotalPage = totalPages,
            HasNext = page < totalPages,
            HasPrev = page > 1
        };
        return data;
    }

    public async Task<BlogPost?> FindOne(int id)
    {
        var foundBlogPost = await this._context.BlogPost
           .FirstOrDefaultAsync(b => b.Id == id);
        return foundBlogPost;
    }

    public async  Task Create(Department department, BlogPostDTO data)
    {
        var blogPost = new BlogPost { Content = data.Content, Author = data.Author };
        blogPost.Department = department;
        blogPost.ExcerptTitle = data.ExcerptTitle;
        blogPost.ExcerptBody = data.ExcerptBody;
        
        var uri = await fileUploader.UploadFileAsync(data.ExcerptImage);
        blogPost.ExcerptImage = uri;
        this._context.BlogPost.Add(blogPost);
        await this._context.SaveChangesAsync();
    }
    public async Task Update(Department department, BlogPost post, BlogPostDTO data)
    {
        post.Content = data.Content;
        post.Author = data.Author;
        post.Department = department;

        await this._context.SaveChangesAsync();
    }

    public async Task Delete(BlogPost post)
    {
        this._context.BlogPost.Remove(post);
        await this._context.SaveChangesAsync();
    }
}