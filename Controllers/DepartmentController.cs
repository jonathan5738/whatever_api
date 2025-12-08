using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using CliniqueBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DepartmentController: ControllerBase
{
    private readonly IDepartmentService departmentService;
    public DepartmentController(AppDbContext context, 
    IDepartmentService departmentService)
    {
        this.departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Department>>> Get()
    {
        var departments = await this.departmentService.FindAll();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> Get([FromRoute] int id)
    {
        var department = await this.departmentService.FindOne(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpGet]
    [Route("/api/[controller]/all")]
    public async Task<ActionResult<DepartmentPagination>> GetAll(int page = 1, int pageSize = 3)
    {
        var data = await this.departmentService
           .FindAll(page, pageSize);
        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] DepartmentDTO data)
    {
        await this.departmentService.Create(data);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] DepartmentDTO data)
    {
        var foundDepartment = await this.departmentService.FindOne(id);
        if (foundDepartment == null)
        {
            return NotFound();
        }
        await this.departmentService.Update(foundDepartment, data);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult>Delete([FromRoute] int id)
    {

        var foundDepartment = await this.departmentService.FindOne(id);
        if (foundDepartment == null)
        {
            return NotFound();
        }
        await this.departmentService.Delete(foundDepartment);
        return NoContent();
    }
}