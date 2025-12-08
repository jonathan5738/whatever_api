using System.ComponentModel.DataAnnotations;
using CliniqueBackend.Models;

namespace CliniqueBackend.Dtos;

public class ScheduleDTO
{
    public int ScheduleId { get; set; }
    
    public string StartHour { get; set; } = default!;

    public string EndHour { get; set; } = default!;

    [Required]
    public string Day { get; set; } = default!;

    [Required]
    public int DayNumber { get; set; }

    [Required]
    public bool IsSelected { get; set; } = false;
    public int? DoctorId { get; set; }
}