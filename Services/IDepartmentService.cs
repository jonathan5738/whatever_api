using CliniqueBackend.Dtos;
using CliniqueBackend.Models;

namespace CliniqueBackend.Services;

public interface IDepartmentService
{
    public Task<List<Department>> FindAll();
    public Task<DepartmentPagination> FindAll(int page = 1, int pageSize = 3);
    public Task<Department?> FindOne(int id);
    public Task Create(DepartmentDTO data);

    public Task Update(Department department, DepartmentDTO data);

    public Task Delete(Department department);

}