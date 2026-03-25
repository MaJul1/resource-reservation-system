using System;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Services;

public class AvailabilityService : IAvailabilityService
{
  public Task<bool> IsReservable(Reservation reservation)
  {
    throw new NotImplementedException();
  }
}
