using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
  private readonly IEmailSender _emailSender;
  public ReservationService(AppDbContext context, IEmailSender emailSender)
  {
    _context = context;
    _emailSender = emailSender;
  }

  public async Task CancelAsync(int id)
  {
    var validStatus = new [] {Enums.Status.PENDING};

    var reservation = await GetByIdOrThrowAsync(id);

    if (!validStatus.Contains(reservation.Status))
      throw new ArgumentException("Only pending status can be cancelled");

    reservation.Status = Enums.Status.CANCELLED;

    await _context.SaveChangesAsync();
 
    _ =  _emailSender.SendEmailAsync(reservation.User.Email, "Cancelled Reservation", "Your reservation has been cancelled.");
  }

  public async Task CreateAsync(CreateReservationRequestDTO request)
  {
    var reservation = request.ToReservation();

    await ValidateReservation(reservation);

    _context.Reservations.Add(reservation);

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
    var reservations = _context.Reservations
      .Include(r => r.User)
      .Include(r => r.Resource);

    var orderedReservation = 
      sortBy == "status" ? reservations.OrderByDescending(e => e.Status) :
      sortBy == "start" ? reservations.OrderByDescending(e => e.Start) :
      sortBy == "end" ? reservations.OrderByDescending (e => e.End) :
        reservations.OrderByDescending(e => e.Id);

    var pagedReservation = orderedReservation
      .Skip((page - 1) * size)
      .Take(size);

    return await pagedReservation
      .Select(r => r.ToReservationDTO())
      .ToListAsync();
  }

  public async Task<ReservationDTO?> GetByIdAsync(int id)
  {
    var reservation = await _context.Reservations
      .Include(r => r.User)
      .Include(r => r.Resource)
      .FirstOrDefaultAsync(r => r.Id == id);

    if (reservation is null)
      throw new KeyNotFoundException($"Reservationwith an id of {id} not found.");

    var dto = reservation.ToReservationDTO();

    return dto;
  }

  public async Task<IEnumerable<ReservationDTO>> GetByResourceId(int resourceId, int page, int size, string sortBy)
  {
    if (!await _context.Resources.AnyAsync(r => r.Id == resourceId))
      throw new KeyNotFoundException($"Resource with an id of {resourceId} not found.");

    var reservations = _context.Reservations
      .Include(r => r.User)
      .Include(r => r.Resource)
      .Where(r => r.ResourceId == resourceId);

    var sorted = 
      sortBy == "status" ? reservations.OrderByDescending(r => r.Status) :
      sortBy == "start" ? reservations.OrderByDescending(r => r.Start) :
      sortBy == "end" ? reservations.OrderByDescending(r => r.End) :
      reservations.OrderByDescending(r => r.Id);

    var paginated = sorted.Skip((page - 1) * size).Take(size);

    var dto = paginated.Select(r => r.ToReservationDTO());

    return await dto.ToListAsync();
  }

  public async Task MarkAsDone(int id)
  {
    var reservation = await GetByIdOrThrowAsync(id);

    if (reservation.Status !=  Enums.Status.ONGOING)
      throw new ArgumentException("Unable to mark reservation done because it is not ongoing. ");
    
    reservation.Status = Enums.Status.DONE;

    await _context.SaveChangesAsync();
  }

  public async Task MarkAsOngoing(int id)
  {
    var reservation = await GetByIdOrThrowAsync(id);

    if (reservation.Status != Enums.Status.PENDING)
      throw new ArgumentException("Unable to mark reservation ongoing because it is not pending. ");

    reservation.Status = Enums.Status.ONGOING;

    await _context.SaveChangesAsync();
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
      var reservation = await _context.Reservations.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id)
      ?? throw new KeyNotFoundException($"Reservation not found with an id of {id}");

      return reservation;
  }

  private async Task<bool> IsReservationTimeAvailable(Reservation reservation)
  {
    var excluded = new []
    {
      Enums.Status.CANCELLED,
    };

    return !await _context.Reservations
      .Where(r => r.Id != reservation.Id)
      .Where(r => !excluded.Contains(r.Status))
      .Where(r => r.ResourceId == reservation.ResourceId)
      .AnyAsync(r => r.Start < reservation.End && reservation.Start < r.End);
  }

  private async Task ValidateReservation(Reservation request)
  {
    if (request.Start >= request.End) 
      throw new ArgumentException("Start date should be earlier than End date");

    if (DateTimeUtils.GetMinutesDifference(request.Start, request.End) < 60)
      throw new ArgumentException("Start and End time should be at least 60 minutes long");

    if (!await _context.Resources.AnyAsync(r => r.Id == request.ResourceId))
      throw new KeyNotFoundException($"Resource with an id of {request.Id} not found");

    if (!await IsReservationTimeAvailable(request))
      throw new ArgumentException($"The reservation overlaps with an existing reservation for resource {request.ResourceId}.");
  }
}
