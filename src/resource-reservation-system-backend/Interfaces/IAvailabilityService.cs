using System;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Interfaces;

public interface IAvailabilityService
{
  Task<bool> IsReservable(Reservation reservation);
}
