using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.DTO.Resource;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/resource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _service;
        public ResourceController (IResourceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateResource([FromBody]CreateResourceRequestDTO request)
        {
            await _service.Create(request);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRessource(int? page, int? size, string? sortBy)
        {
            var result = await _service.GetResources(page: page ?? 1, size: size ?? 20, sortBy: sortBy = "id");

            return Ok(result);
        }
    }
}
