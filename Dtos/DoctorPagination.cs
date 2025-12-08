using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliniqueBackend.Models;

namespace CliniqueBackend.Dtos;

public class DoctorData
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(50)")]
    [Required]
    public string FirstName { get; set; } = default!;

    [Column(TypeName = "varchar(50)")]
    public string? MiddleName { get; set; }

    [Column(TypeName = "varchar(75)")]
    [Required]
    public string LastName { get; set; } = default!;

    public int DepartmentId { get; set; }

    public String Department {get; set;} = default!;
}
public class DoctorPagination
{
    public List<DoctorData> Data {get; set;} = default!;
    
    public int TotalPage { get; set; }
    public bool HasPrev { get; set; }
    public bool HasNext { get; set; }
}