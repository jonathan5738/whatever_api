using CliniqueBackend.Data;
using CliniqueBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ScheduleController: ControllerBase
{
    private readonly AppDbContext _context;
    public ScheduleController(AppDbContext context) => this._context = context;

    [HttpGet("{doctorId}")]
    public async Task<ActionResult<List<Schedule>>> Get([FromRoute] int doctorId)
    {
        List<Schedule> schedules = await this._context.Schedule
          .Where(s => s.DoctorId == doctorId).ToListAsync();
        return Ok(schedules);
    }
}