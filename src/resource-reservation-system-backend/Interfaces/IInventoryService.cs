using System;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Interfaces;

public interface IInventoryService
{
  Task<IEnumerable<AvailableItem>> GetAllItems();
  Task<AvailableItem?> GetItemById(int id);
  Task DeductItem(int itemId, int quantity);
}
