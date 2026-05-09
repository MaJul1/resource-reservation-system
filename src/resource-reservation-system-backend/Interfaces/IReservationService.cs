using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Interfaces;

public interface IReservationService
{
  Task<IEnumerable<SummaryReservationDTO>> GetAllAsync();
  Task<IEnumerable<SummaryReservationDTO>> GetAllAsync(int page, int size, string sortBy);
  Task<IEnumerable<SummaryReservationDTO>> GetByResourceId(int id, int page, int size, string sortBy);
  Task<DetailedReservationDTO?> GetByIdAsync(int id);
  Task CreateAsync(CreateReservationRequestDTO request);
  Task MoveAsync(MoveReservationRequestDTO request);
  Task MarkAsOngoing(int id);
  Task MarkAsDone(int id);
  Task CancelAsync(int id);
}
