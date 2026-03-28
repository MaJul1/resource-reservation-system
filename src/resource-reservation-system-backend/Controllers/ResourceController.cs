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

        [HttpGet("get-resources")]
        public async Task<IActionResult> GetRessource(int? page, int? size, string? sortBy)
        {
            var result = await _service.GetResources(page: page ?? 1, size: size ?? 20, sortBy: sortBy = "id");

            return Ok(result);
        }

        [HttpGet("get-resources-info")]
        public async Task<IActionResult> GetResourceNameAndId()
        {
            var result = await _service.GetResourceNamesAndId();

            return Ok(result);
        }

        [HttpGet("get-resrouce-by-id/{id}")]
        public async Task<IActionResult> GetResourceById(int id)
        {
            var result = await _service.GetResourceById(id);

            return Ok(result);
        }
    }
}
