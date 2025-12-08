using System.ComponentModel.DataAnnotations.Schema;
using CliniqueBackend.Models;

namespace CliniqueBackend.Dtos;

public class DepartmentPagination
{

    public List<Department> Data { get; set; } = default!;

    public int TotalPage { get; set; }
    public bool HasPrev { get; set; }
    public bool HasNext { get; set; }
}