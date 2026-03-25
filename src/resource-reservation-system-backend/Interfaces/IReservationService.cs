using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Interfaces;

public interface IReservationService
{
  Task<List<Reservation>> GetAllAsync();
  Task<List<Reservation>> GetAllAsync(int? page, int? size, string? sortBy);
  Task<Reservation> GetByIdAsync(int id);
  Task CreateAsync(CreateReservationRequestDTO request);
  Task MoveAsync(MoveReservationRequestDTO request);
  Task ApproveAsync(int id);
  Task DenyAsync(int id);
  Task CancelAsync(int id);
}
