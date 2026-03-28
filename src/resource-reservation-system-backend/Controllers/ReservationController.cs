using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("get-reservations")]
        public async Task<IActionResult> GetReservations(int? size, int? page, string? sortBy)
        {   
            var result = await _reservationService.GetAllAsync(page: page ?? 1,size: size ?? 20 , sortBy ?? "id");

            return Ok(result);
        }
        [HttpGet("get-reservation/{id}")]
        public async Task<IActionResult> GetReservationById(int id) {
            var result = await _reservationService.GetByIdAsync(id);

            return Ok(result);
        }
        
        [HttpPost("create-reservation")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDTO request)
        {
            await _reservationService.CreateAsync(request);
            
            return Ok();
        }

        [HttpPost("approve-reservation")]
        public async Task<IActionResult> ApproveReservation(int id)
        {
            await _reservationService.ApproveAsync(id);

            return Ok();
        } 

        [HttpPost("cancel-reservation")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            await _reservationService.CancelAsync(id);

            return Ok();
        }

        [HttpPost("deny-reservation")]
        public async Task<IActionResult> DenyReservation(int id)
        {
            await _reservationService.DenyAsync(id);

            return Ok();
        }

        [HttpPost("move-reservation")]
        public async Task<IActionResult> MoveReservation([FromBody] MoveReservationRequestDTO request)
        {
            await _reservationService.MoveAsync(request);

            return Ok();
        }

        
    }
}
