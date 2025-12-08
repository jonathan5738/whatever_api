using CliniqueBackend.Models;

namespace CliniqueBackend.Dtos;

public class ContactMessagePagination
{
    public List<ContactMessage> Data { get; set; } = default!;

    public int TotalPage { get; set; }
    public bool HasPrev { get; set; }
    public bool HasNext { get; set; }
}