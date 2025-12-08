using CliniqueBackend.Data;
using CliniqueBackend.Dtos;
using CliniqueBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CliniqueBackend.Services;

public class DepartmentService: IDepartmentService
{
    private readonly AppDbContext _context;
    public DepartmentService(AppDbContext context)
    {
        this._context = context;
    }
    public async Task<List<Department>> FindAll()
    {
        return await this._context.Department.ToListAsync();
    }
    public async Task<DepartmentPagination> FindAll(int page = 1, int pageSize = 3)
    {
        var totalCount = await this._context.Department.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        var result = await this._context.Department
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Include(department => department.Doctors)
             .ThenInclude(doctor => doctor.Schedules)
             .ToListAsync();
        var data = new DepartmentPagination
        {
            Data = result,
            TotalPage = totalPages,
            HasNext = page < totalPages,
            HasPrev = page > 1
        };
        return data;
    }
    public async Task<Department?> FindOne(int id)
    {
        var department = await this._context.Department
        .FirstOrDefaultAsync(d => d.Id == id);
        return department;
    }
    public async Task Create(DepartmentDTO data)
    {
        var department = new Department { Name = data.Name };
        this._context.Department.Add(department);
        await this._context.SaveChangesAsync();
    }

    public async Task Update(Department department, DepartmentDTO data)
    {
        department.Name = data.Name;
        await this._context.SaveChangesAsync();
    }

    public async Task Delete(Department department)
    {
        this._context.Department.Remove(department);
        await this._context.SaveChangesAsync();
    }
}