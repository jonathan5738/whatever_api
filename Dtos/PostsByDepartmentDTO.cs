namespace CliniqueBackend.Dtos;

public class PostsByDepartmentDTO
{
    public int Id {get; set;}
    public string Name {get; set;} = default!;
    public IList<PostExcerpt> Posts {get; set;} = default!;
}