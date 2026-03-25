using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Mapper;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Persistence;

namespace resource_reservation_system_backend.Services;

public class ReservationService : IReservationService
{
  private readonly AppDbContext _context;
  public ReservationService(AppDbContext context)
  {
    _context = context;
  }

  public Task ApproveAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task CancelAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task CreateAsync(CreateReservationRequestDTO request)
  {
    throw new NotImplementedException();
  }

  public Task DenyAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<ReservationDTO>> GetAllAsync()
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<ReservationDTO>> GetAllAsync(int? page, int? size, string? sortBy)
  {
    throw new NotImplementedException();
  }

  public Task<ReservationDTO> GetByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public Task MoveAsync(MoveReservationRequestDTO request)
  {
    throw new NotImplementedException();
  }
}
