using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Services;

public class InventoryService : IInventoryService
{
  private readonly HttpClient _httpClient;
  public InventoryService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task DeductItem(int itemId, int quantity)
  {
    var form = new MultipartFormDataContent
    {
      { new StringContent(itemId.ToString()), "inventory_id" },
      { new StringContent(quantity.ToString()), "issue_qty" }
    };

    var result = await _httpClient.PostAsync("https://icis-inventory.onrender.com/includes/api/api.php?action=issue_item", form);

    var json = await result.Content.ReadAsStringAsync();

    var doc = JsonDocument.Parse(json);

    string status = doc.RootElement.GetProperty("status").GetString()!;

    if (status == "error")
    {
      string message = doc.RootElement.GetProperty("message").GetString()!;
      throw new KeyNotFoundException(message);
    }
  }

  public async Task<IEnumerable<AvailableItem>> GetAllItems()
  {
    var response = await _httpClient.GetFromJsonAsync<Response<IEnumerable<AvailableItem>>>("https://icis-inventory.onrender.com/includes/api/api.php?action=get_inventory");

    return response!.Data ?? [];
  }

  public async Task<AvailableItem?> GetItemById(int id)
  {
    var response = await _httpClient.GetFromJsonAsync<Response<IEnumerable<AvailableItem>>>("https://icis-inventory.onrender.com/includes/api/api.php?action=get_inventory");

    var item = response?.Data.FirstOrDefault(i => i.Inventory_id == id);

    return item;

  }

  private record Response<T>
  {
    public string Status { get; set; } = null!;
    public T Data { get; set; } = default!;
  }
}
