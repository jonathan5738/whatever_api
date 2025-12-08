using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly AppDbContext _context;
    public DoctorController(AppDbContext context) => this._context = context;

    [HttpGet]
    public async Task<ActionResult<List<Doctor>>> Get()
    {
        var doctors = await this._context.Doctor
        .Include(d => d.Schedules)
        .ToListAsync();
        return Ok(doctors);
    }

    [HttpGet]
    [Route("/api/[controller]/all")]
    public async Task<ActionResult<DoctorPagination>> GetAll(int page = 1, int pageSize = 4)
    {
        var totalCount = await this._context.Doctor.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        var result = await this._context.Doctor
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Include(doctor => doctor.Department)
             .Select(doctor => new DoctorData
             {
                 Id = doctor.Id,
                 FirstName = doctor.FirstName,
                 LastName = doctor.LastName,
                 MiddleName = doctor.MiddleName,
                 DepartmentId = doctor.DepartmentId,
                 Department = doctor.Department.Name
             })
             .ToListAsync();
        var data = new DoctorPagination
        {
            Data = result,
            TotalPage = totalPages,
            HasNext = page < totalPages,
            HasPrev = page > 1
        };
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Doctor>> Get([FromRoute] int id)
    {
        var foundDoctor = await this._context
           .Doctor
           .Include(d => d.Schedules)
           .FirstOrDefaultAsync(d => d.Id == id);

        if (foundDoctor == null) return NotFound();
        return Ok(foundDoctor);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] DoctorDTO data)
    {
        var department = await this._context
           .Department.FirstOrDefaultAsync(d => d.Id == data.DepartmentId);

        if (department == null)
        {
            return NotFound();
        }
        var doctor = new Doctor
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Department = department
        };
        if (data.MiddleName != null)
        {
            doctor.MiddleName = data.MiddleName;
        }
        this._context.Doctor.Add(doctor);
        await this._context.SaveChangesAsync();

        if (data.Schedules.Count > 0)
        {
            // save each doctor schedule in schedule table
            var foundDoctor = await this._context.Doctor
            .FirstOrDefaultAsync(d => d.FirstName == data.FirstName
            && d.LastName == data.LastName);

            if (foundDoctor == null)
            {
                return BadRequest();
            }
            foreach (ScheduleDTO schedule in data.Schedules)
            {
                var s = new Schedule
                {
                    Day = schedule.Day,
                    StartHour = schedule.StartHour,
                    EndHour = schedule.EndHour,
                    DayNumber = schedule.DayNumber,
                    IsSelected = schedule.IsSelected
                };
                s.Doctor = foundDoctor;
                this._context.Schedule.Add(s);
            }
            await this._context.SaveChangesAsync();
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] DoctorDTO data)
    {
        var foundDoctor = await this._context.Doctor
          .Include(d => d.Schedules)
          .FirstOrDefaultAsync(d => d.Id == id);
        if (foundDoctor == null)
        {
            return NotFound();
        }
        var department = await this._context
           .Department.FirstOrDefaultAsync(d => d.Id == data.DepartmentId);

        if (department == null)
        {
            return NotFound();
        }
        foundDoctor.FirstName = data.FirstName;
        foundDoctor.LastName = data.LastName;
        foundDoctor.Department = department;

        if (data.MiddleName != null)
        {
            foundDoctor.MiddleName = data.MiddleName;
        }
        if (data.Schedules.Count < foundDoctor.Schedules.Count)
        {
            var ids = (from s in data.Schedules
                       select s.ScheduleId).ToList();
            var scheduleToDelete = foundDoctor.Schedules
            .Where(s => !ids.Contains(s.Id))
            .ToList();
            this._context.Schedule.RemoveRange(scheduleToDelete);
        }
        if (data.Schedules.Count == 0)
        {
            return BadRequest();
        }
        if (data.Schedules.Count > 0)
        {
            foreach (ScheduleDTO scheduleDTO in data.Schedules)
            {
                // a scheduleDTO is new if it has a scheduleId of 0
                if (scheduleDTO.ScheduleId == 0)
                {
                    var schedule = new Schedule
                    {
                        StartHour = scheduleDTO.StartHour,
                        EndHour = scheduleDTO.EndHour,
                        Day = scheduleDTO.Day
                    };
                    schedule.Doctor = foundDoctor;
                    this._context.Schedule.Add(schedule);
                    continue;
                }

                var selectedSchedule = foundDoctor.Schedules
                  .Where(s => s.Id == scheduleDTO.ScheduleId)
                  .SingleOrDefault();
                if (selectedSchedule == null)
                {
                    return BadRequest();
                }
                selectedSchedule.Day = scheduleDTO.Day;
                selectedSchedule.StartHour = scheduleDTO.StartHour;
                selectedSchedule.EndHour = scheduleDTO.EndHour;
                selectedSchedule.IsSelected = scheduleDTO.IsSelected;
            }
        }
        await this._context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/Department/update/{departmentId}")]
    public async Task<ActionResult> Patch([FromRoute] int id, [FromRoute] int departmentId)
    {
        var foundDoctor = await this._context.Doctor.FirstOrDefaultAsync(d => d.Id == id);
        var department = await this._context.Department.FirstOrDefaultAsync(d => d.Id == departmentId);

        if (foundDoctor == null || department == null)
        {
            return NotFound();
        }
        foundDoctor.Department = department;
        await this._context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var foundDoctor = await this._context.Doctor
            .FirstOrDefaultAsync(doctor => doctor.Id == id);

        if (foundDoctor == null)
        {
            return NotFound();
        }
        this._context.Doctor.Remove(foundDoctor);
        await this._context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete()
    {
        var doctors = await this._context.Doctor.ToListAsync();
        foreach (var doctor in doctors)
        {
            this._context.Doctor.Remove(doctor);
        }
        await this._context.SaveChangesAsync();
        return Ok();
    }
}

/*{
  "firstName": "string",
  "middleName": "string",
  "lastName": "string",
  "departmentId": 0,
  "schedules": [
    {
      "scheduleId": 0,
      "startHour": "string",
      "endHour": "string",
      "day": "string",
      "doctorId": 0
    }
  ]
}*/

/*
 {
    "id": 4,
    "firstName": "Jonathan",
    "middleName": "JUNIOR",
    "lastName": "naka",
    "departmentId": 2,
    "department": null,
    "createdAt": "2025-10-19T05:31:13.401625",
    "updatedAt": "2025-10-19T05:50:44.986846",
    "schedules": [
      {
        "id": 1,
        "startHour": "03:34:00",
        "endHour": "19:45:00",
        "day": "Tuesday",
        "doctorId": 4,
        "doctor": null,
        "createdAt": "2025-10-19T05:31:13.488242",
        "updatedAt": "2025-10-19T05:51:17.193233"
      }
    ]
  }

public class Blog 
{
    private int Id {get;set;}
    public string Title {get;set;}
    public string Content {get; set;}
}

public class AppDbContext: DbContext
{
    public DbSet<Blog> Blog {get;set;}
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
}
*/