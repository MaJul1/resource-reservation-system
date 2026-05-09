using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.DTO.Facility;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/facility")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _service;
        public FacilityController (IFacilityService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody]CreateFacilityRequestDTO request)
        {
            await _service.Create(request);

            return Ok();
        }

        [HttpGet("get-facilities")]
        public async Task<IActionResult> GetFacility(int? page, int? size, string? sortBy)
        {
            var result = await _service.GetFacility(page: page ?? 1, size: size ?? 20, sortBy: sortBy = "id");

            return Ok(result);
        }

        [HttpGet("get-facilities-info")]
        public async Task<IActionResult> GetFacilityNameAndId()
        {
            var result = await _service.GetFacilityNamesAndId();

            return Ok(result);
        }

        [HttpGet("get-facility-by-id/{id}")]
        public async Task<IActionResult> GetFacilityById(int id)
        {
            var result = await _service.GetFacilityById(id);

            return Ok(result);
        }
    }
}
