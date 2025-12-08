using System.ComponentModel.DataAnnotations;

namespace CliniqueBackend.Dtos;

public class BlogPostDTO
{
    [Required]
    public string Content { get; set; } = default!;
    [Required]
    public string Author { get; set; } = default!;

    [Required]
    public string ExcerptTitle { get; set; } = default!;

    public IFormFile ExcerptImage {get; set;} = default!;

    [Required]
    public string ExcerptBody { get; set; } = default!;
    [Required]
    public int DepartmentId { get; set; }
}