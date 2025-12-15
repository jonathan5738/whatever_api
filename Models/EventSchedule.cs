using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliniqueBackend.Models;

[Table("event_schedules")]
public class EventSchedule
{
    public int Id { get; set; }

    [Required]
    public string Date { get; set; } = default!;

    [Required]
    public string StartHour { get; set; } = default!;

    [Required]
    public string EndHour { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;

}