using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.DTO.Department;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Mapper;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Persistence;

namespace resource_reservation_system_backend.Services;

public class DepartmentService : IDepartmentService
{
  private readonly AppDbContext _context;
  public DepartmentService(AppDbContext context)
  {
    _context = context;
  }
  public async Task CreateDepartment(string name)
  {
    var department = new Department
    {
      Name = name
    };

    _context.Departments.Add(department);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteDepartment(int id)
  {
    var department = await _context.Departments.FindAsync(id)
      ?? throw new KeyNotFoundException("Department not found");
    
    _context.Departments.Remove(department);
    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<SummaryDepartmentDTO>> GetAllDepartments()
  {
    var departments = await _context.Departments.ToListAsync();
    return departments.Select(d => d.ToDepartmentDTO());
  }

  public async Task<DetailedDepartmentDTO> GetDepartmentById(int id)
  {
    var department = await _context.Departments.FindAsync(id);
    return department?.ToDetailedDepartmentDTO() ?? throw new KeyNotFoundException("Department not found");
  }

  public Task UpdateDepartment(int id, string name)
  {
    var department = _context.Departments.Find(id) ?? throw new KeyNotFoundException("Department not found");
    department.Name = name;
    _context.Departments.Update(department);
    return _context.SaveChangesAsync();
  }
}
