using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Persistence;

namespace resource_reservation_system_backend.Services;

public class ReservationService
{
  private readonly AppDbContext _context;
  public ReservationService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<Reservation>> GetReservationAsync()
  {
    return await _context.Reservations.OrderBy(e => e.Id).ToListAsync();
  }

  public async Task<List<Reservation>> GetReservationAsync(int page, int size)
  {
    var reservations = await GetReservationAsync();

    var filtered = reservations
      .Skip((page - 1) * size)
      .Take(size);

    return [.. filtered];
  }
}
