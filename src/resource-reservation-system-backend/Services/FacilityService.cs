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
  public FacilityService (AppDbContext context, IInventoryService inventoryService)
  {
    _context = context;
    _inventoryService = inventoryService;
  }

  public async Task Create(CreateFacilityRequestDTO request)
  {
    var resource = request.ToResource();

    var items = await _inventoryService.GetAllItems();

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

  public async Task<FacilityDTO> GetFacilityById(int id)
  {
    var resource = await _context.Facilities.FindAsync(id) ?? 
      throw new KeyNotFoundException($"Resource with an id of {id} not found.");
    
    return resource.ToFacilityDTO();
  }

  public async Task<IEnumerable<NameAndIdDTO>> GetFacilityNamesAndId()
  {
    return await 
      _context.Facilities.Select(r => r.ToNameAndIdDTO()).ToListAsync();
  }

  public async Task<IEnumerable<FacilityDTO>> GetFacility(int page, int size, string sortBy)
  {
    var resources = sortBy == "name" ? _context.Facilities.OrderBy(e => e.Name) : 
    sortBy == "type" ? _context.Facilities.OrderBy(e => e.Type) :
    _context.Facilities.OrderBy(e => e.Id);

    var pagedResource = resources.Skip((page - 1) * size).Take(size);

    var dto = pagedResource.Select(e => e.ToFacilityDTO());
    return await dto.ToListAsync();
  }
}
