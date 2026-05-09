using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.DTO.Facility;
using resource_reservation_system_backend.Mapper;
using resource_reservation_system_backend.Persistence;
using resource_reservation_system_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace resource_reservation_system_backend.Services;

public class FacilityService : IFacilityService
{
  private readonly AppDbContext _context;
  private readonly IInventoryService _inventoryService;
  public FacilityService(AppDbContext context, IInventoryService inventoryService)
  {
    _context = context;
    _inventoryService = inventoryService;
  }

  public async Task Create(CreateFacilityRequestDTO request)
  {
    var resource = request.ToFacility();

    var items = await _inventoryService.GetAllItems();

    var departments = await _context.Departments.Where(d => request.DepartmentIds.Contains(d.Id)).ToListAsync();
    if (departments.Count != request.DepartmentIds.Count())
    {
      var existingDepartmentIds = departments.Select(d => d.Id);
      var nonExistingDepartmentIds = request.DepartmentIds.Where(id => !existingDepartmentIds.Contains(id));
      throw new KeyNotFoundException($"Departments with the following ids were not found: {string.Join(", ", nonExistingDepartmentIds)}");
    }

    foreach (var item in request.ItemsAllocated)
    {
      var inventoryItem = items.FirstOrDefault(i => i.Inventory_id == item.InventoryId)
        ?? throw new KeyNotFoundException($"Item with an id of {item.InventoryId} not found in inventory.");
      if (inventoryItem.Quantity < item.Quantity)
      {
        throw new InvalidOperationException($"Not enough quantity for item with an id of {item.InventoryId}. Available quantity: {inventoryItem.Quantity}");
      }

      resource.ItemsAllocated.Add(new ItemAllocation
      {
        InventoryId = item.InventoryId,
        Name = inventoryItem.Name,
        Quantity = item.Quantity
      });
    }

    _context.Facilities.Add(resource);

    foreach (var item in request.ItemsAllocated)
    {
      await _inventoryService.DeductItem(item.InventoryId, item.Quantity);
    }

    await _context.SaveChangesAsync();
  }

  public async Task<DetailedFacilityDTO> GetFacilityById(int id)
  {
    var resource = await _context.Facilities
      .Include(f => f.ItemsAllocated)
      .Include(f => f.Departments)
      .Where(f => f.Id == id)
      .AsSplitQuery()
      .FirstOrDefaultAsync()
      ?? throw new KeyNotFoundException($"Resource with an id of {id} not found.");

    return resource.ToDetailedFacilityDTO();
  }

  public async Task<IEnumerable<NameAndIdDTO>> GetFacilityNamesAndId()
  {
    return await _context.Facilities.Select(r => r.ToNameAndIdDTO()).ToListAsync();
  }

  public async Task<IEnumerable<SummarizedFacilityDTO>> GetFacility(int page, int size, string sortBy)
  {
    var resources = sortBy == "name" ?
    _context.Facilities.OrderBy(e => e.Name) : sortBy == "type" ?
    _context.Facilities.OrderBy(e => e.Type) :
    _context.Facilities.OrderBy(e => e.Id);

    var pagedResource = resources.Skip((page - 1) * size).Take(size);

    var dto = pagedResource.Select(e => e.ToSummarizedFacilityDTO());
    return await dto.ToListAsync();
  }

  public async Task Update(UpdateFacilityRequestDTO request)
  {
    var facility = await _context.Facilities.FindAsync(request.Id) ??
      throw new KeyNotFoundException($"Resource with an id of {request.Id} not found.");

    var departments = await _context.Departments.Where(d => request.DepartmentIds.Contains(d.Id)).ToListAsync();
    if (departments.Count != request.DepartmentIds.Count())
    {
      var existingDepartmentIds = departments.Select(d => d.Id);
      var nonExistingDepartmentIds = request.DepartmentIds.Where(id => !existingDepartmentIds.Contains(id));
      throw new KeyNotFoundException($"Departments with the following ids were not found: {string.Join(", ", nonExistingDepartmentIds)}");
    }

    facility.Name = request.Name;
    facility.Type = request.Type;
    facility.Location = request.Location;
    facility.Capacity = request.Capacity;
    facility.Description = request.Description;
    facility.Departments = departments;

    _context.Facilities.Update(facility);
    await _context.SaveChangesAsync();
  }

  public async Task Delete(int id)
  {
    var facility = await _context.Facilities.FindAsync(id) ??
      throw new KeyNotFoundException($"Resource with an id of {id} not found.");

    _context.Facilities.Remove(facility);
    await _context.SaveChangesAsync();
  }
}
