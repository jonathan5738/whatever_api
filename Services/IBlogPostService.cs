using CliniqueBackend.Dtos;
using CliniqueBackend.Models;

namespace CliniqueBackend.Services;

public interface IBlogPostService
{
    public Task<BlogPostPagination> FindAll(int page, int pageSize);
    public Task<BlogPost?> FindOne(int id);
    public Task Create(Department department, BlogPostDTO data);
    public Task Update(Department department, BlogPost post, BlogPostDTO data);

    public Task Delete(BlogPost post);
}