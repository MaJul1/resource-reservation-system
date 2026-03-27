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

  public async Task ApproveAsync(int id)
  {
    var reservation = await GetByIdThrowErrorIfNotFound(id);

    if (reservation.Status != Enums.Status.PENDING) 
      throw new ArgumentException($"Non pending reservation cannot be approved.");
    
    reservation.Status = Enums.Status.APPROVED;

    await _context.SaveChangesAsync();
  }

  public async Task CancelAsync(int id)
  {
    var reservation = await GetByIdThrowErrorIfNotFound(id);

    reservation.Status = Enums.Status.CANCELLED;

    await _context.SaveChangesAsync();
  }

  public async Task CreateAsync(CreateReservationRequestDTO request)
  {
    var reservation = request.ToReservation();

    if (!await IsAvailable(reservation))
    {
      throw new ArgumentException($"The reservation overlaps with an existing reservation.");
    }

    _context.Reservations.Add(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task DenyAsync(int id)
  {
    var reservation = await GetByIdThrowErrorIfNotFound(id);

    if (reservation.Status != Enums.Status.PENDING) 
      throw new ArgumentException($"Non pending reservation cannot be denied.");
    
    reservation.Status = Enums.Status.DENIED;

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
  {
    return await Task.FromResult(_context.Reservations.Include(r => r.User).Select(r => r.ToReservationDTO()));
  }

  public async Task<IEnumerable<ReservationDTO>> GetAllAsync(int page = 1, int size = 20, string sortBy = "id")
  {
    var reservations = _context.Reservations.Include(r => r.User);

    var orderedReservation = 
      sortBy == "status" ? reservations.OrderBy(e => e.Status) :
      sortBy == "start" ? reservations.OrderBy(e => e.Start) :
      sortBy == "end" ? reservations.OrderBy (e => e.End) :
        reservations.OrderBy(e => e.Id);

    var pagedReservation = reservations
      .Skip((page - 1) * size)
      .Take(size);

    var dto = pagedReservation.Select(r => r.ToReservationDTO());

    return await Task.FromResult(dto);
  }

  public async Task<ReservationDTO?> GetByIdAsync(int id)
  {
    var reservation = await _context.Reservations.FindAsync(id);

    if (reservation is null)
    {
      return null;
    }

    var dto = reservation.ToReservationDTO();

    return dto;
  }

  public Task MoveAsync(MoveReservationRequestDTO request)
  {
    throw new NotImplementedException();
  }

  private async Task<Reservation> GetByIdThrowErrorIfNotFound(int id)
  {
      var reservation = await _context.Reservations.FindAsync(id)
      ?? throw new ArgumentNullException($"Reservation not found with an id of {id}");

      return reservation;
  }

  private async Task<bool> IsAvailable(Reservation reservation)
  {
    var overlappingReservations = await _context.Reservations
      .Where(r => r.Id != reservation.Id)
      .Where(r => r.Start < reservation.End && reservation.Start < r.End)
      .ToListAsync();

    return overlappingReservations.Count == 0;
  }
}
