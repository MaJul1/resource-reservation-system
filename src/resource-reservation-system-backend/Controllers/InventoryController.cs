using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;
        public InventoryController(IInventoryService service)
        {
            _service = service;
        }
        
        [HttpGet("get-available-items")]
        public async Task<ActionResult<IEnumerable<AvailableItem>>> GetAvailableItems()
        {
            var items = await _service.GetAllItems();

            return Ok(items);
        }
    }
}
