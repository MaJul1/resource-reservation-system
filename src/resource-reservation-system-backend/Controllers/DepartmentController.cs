using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.Interfaces;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
  {
    private readonly IDepartmentService _service;
    public DepartmentController(IDepartmentService service)
    {
      _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
      var departments = await _service.GetAllDepartments();
      return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
      var department = await _service.GetDepartmentById(id);
      return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] string name)
    {
      await _service.CreateDepartment(name);
      return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] string name)
    {
      await _service.UpdateDepartment(id, name);
      return Ok();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
      await _service.DeleteDepartment(id);
      return Ok();
    }
  }
}
