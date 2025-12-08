using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliniqueBackend.Models;

[Table("schedules")]
public class Schedule
{
    public int Id { get; set; }

    [Required]
    public string StartHour { get; set; } = default!;

    [Required]
    public string EndHour { get; set; } = default!;

    public string Day { get; set; } = default!;

    public int DayNumber { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = default!;

    public bool IsSelected { get; set; } = false;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}
