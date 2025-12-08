using System.ComponentModel.DataAnnotations.Schema;
using CliniqueBackend.Models;

namespace CliniqueBackend.Dtos;

public class EventPagination
{

    public List<Event> Data { get; set; } = default!;

    public int TotalPage { get; set; }
    public bool HasPrev { get; set; }
    public bool HasNext { get; set; }
}