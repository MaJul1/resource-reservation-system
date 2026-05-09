using resource_reservation_system_backend.DTO.ItemAllocation;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class ItemAllocationMapper
{
  public static ItemAllocationDTO ToItemAllocationDTO(this ItemAllocation itemAllocation)
  {
     ItemAllocationDTO dto = new ()
    {
      Id = itemAllocation.Id,
      InventoryId = itemAllocation.InventoryId,
      Name = itemAllocation.Name,
      Quantity = itemAllocation.Quantity
    };

    return dto;
  }
}
