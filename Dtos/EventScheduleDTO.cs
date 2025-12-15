using System.ComponentModel.DataAnnotations;
namespace CliniqueBackend.Dtos;

public class EventScheduleDTO
{
    public int ScheduleId { get; set; } 
    [Required]
    public string Date { get; set; } = default!;

    [Required]
    public string StartHour { get; set; } = default!;

    [Required]
    public string EndHour { get; set; } = default!;
    public int EventId { get; set; }
}