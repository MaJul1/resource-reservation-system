using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Mapper;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Persistence;
using resource_reservation_system_backend.Utils;

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
    var reservation = await GetByIdOrThrowAsync(id);

    if (reservation.Status != Enums.Status.PENDING) 
      throw new ArgumentException($"Non pending reservation cannot be approved.");
    
    reservation.Status = Enums.Status.APPROVED;

    await _context.SaveChangesAsync();
  }

  public async Task CancelAsync(int id)
  {
    var validStatus = new [] {Enums.Status.PENDING, Enums.Status.APPROVED};

    var reservation = await GetByIdOrThrowAsync(id);

    if (!validStatus.Contains(reservation.Status))
      throw new ArgumentException("Only pending and approved status can be cancelled");

    reservation.Status = Enums.Status.CANCELLED;

    await _context.SaveChangesAsync();
  }

  public async Task CreateAsync(CreateReservationRequestDTO request)
  {
    var reservation = request.ToReservation();

    await ValidateReservation(reservation);

    _context.Reservations.Add(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task DenyAsync(int id)
  {
    var reservation = await GetByIdOrThrowAsync(id);

    if (reservation.Status != Enums.Status.PENDING) 
      throw new ArgumentException($"Non pending reservation cannot be denied.");
    
    reservation.Status = Enums.Status.DENIED;

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
  {
    return await _context.Reservations
      .Include(r => r.User)
      .Select(r => r.ToReservationDTO())
      .ToListAsync();
  }

  public async Task<IEnumerable<ReservationDTO>> GetAllAsync(int page, int size, string sortBy)
  {
    var reservations = _context.Reservations.Include(r => r.User);

    var orderedReservation = 
      sortBy == "status" ? reservations.OrderBy(e => e.Status) :
      sortBy == "start" ? reservations.OrderBy(e => e.Start) :
      sortBy == "end" ? reservations.OrderBy (e => e.End) :
        reservations.OrderBy(e => e.Id);

    var pagedReservation = orderedReservation
      .Skip((page - 1) * size)
      .Take(size);

    return await pagedReservation
      .Select(r => r.ToReservationDTO())
      .ToListAsync();
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

  public async Task MoveAsync(MoveReservationRequestDTO request)
  {
    var reservation = await GetByIdOrThrowAsync(request.Id);

    reservation.Start = request.NewStart;
    reservation.End = request.NewEnd;

    if (reservation.Status != Enums.Status.PENDING)
      throw new ArgumentException("Unable to move non pending reservation.");

    await ValidateReservation(reservation);

    await _context.SaveChangesAsync();
  }

  private async Task<Reservation> GetByIdOrThrowAsync(int id)
  {
      var reservation = await _context.Reservations.FindAsync(id)
      ?? throw new KeyNotFoundException($"Reservation not found with an id of {id}");

      return reservation;
  }

  private async Task<bool> IsReservationTimeAvailable(Reservation reservation)
  {
    var excluded = new []
    {
      Enums.Status.CANCELLED,
      Enums.Status.DENIED
    };

    return await _context.Reservations
      .Where(r => r.Id != reservation.Id)
      .Where(r => !excluded.Contains(r.Status))
      .AnyAsync(r => r.Start < reservation.End && reservation.Start < r.End);
  }

  private async Task ValidateReservation(Reservation request)
  {
    if (request.Start >= request.End) 
      throw new ArgumentException("Start date should be earlier than End date");

    // if (DateTimeUtils.GetMinutesDifference(request.Start, request.End) < 60)
    //   throw new ArgumentException("Start and End time should be at least 60 minutes long");

    if (!await IsReservationTimeAvailable(request))
    {
      throw new ArgumentException($"The reservation overlaps with an existing reservation.");
    }
  }
}
