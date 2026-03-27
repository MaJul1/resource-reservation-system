using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("create-reservation")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDTO request)
        {
            await _reservationService.CreateAsync(request);
            
            return Ok();
        }

        [HttpGet("get-reservations")]
        public async Task<IActionResult> GetReservations(int? size, int? page, string? sortBy)
        {   
            var result = await _reservationService.GetAllAsync(page: page ?? 1,size: size ?? 20 , sortBy ?? "id");

            return Ok(result);
        }
    }
}
