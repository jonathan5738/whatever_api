namespace CliniqueBackend.Dtos;

public class PostExcerpt
{
    public int Id {get; set;}

    public string DepartmentName {get; set;} = default!;
    public string ExcerptTitle {get; set;} = default!;
    public string ExcerptBody {get; set;} = default!;
    public string ExcerptImage {get; set;} = default!;
    public string Author {get; set;} = default!;
}