using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.DTO.Resource;
using resource_reservation_system_backend.Mapper;
using resource_reservation_system_backend.Persistence;
using resource_reservation_system_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace resource_reservation_system_backend.Services;

public class ResourceService : IResourceService
{
  private readonly AppDbContext _context;
  public ResourceService (AppDbContext context)
  {
    _context = context;
  }

  public async Task Create(CreateResourceRequestDTO request)
  {
    var resource = request.ToResource();

    _context.Resources.Add(resource);

    await _context.SaveChangesAsync();
  }

  public Task<ResourceDTO> GetResourceById(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<ResourceDTO>> GetResources(int page, int size, string sortBy)
  {
    var resources = sortBy == "name" ? _context.Resources.OrderBy(e => e.Name) : 
    sortBy == "type" ? _context.Resources.OrderBy(e => e.Type) :
    _context.Resources.OrderBy(e => e.Id);

    var pagedResource = resources.Skip((page - 1) * size).Take(size);

    var dto = pagedResource.Select(e => e.ToResourceDTO());
    return await dto.ToListAsync();
  }
}
