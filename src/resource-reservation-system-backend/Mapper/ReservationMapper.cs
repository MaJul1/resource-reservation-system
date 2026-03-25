using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Utils;

namespace resource_reservation_system_backend.Mapper;

public static class ReservationMapper
{
  public static ReservationDTO ToReservationDTO(this Reservation reservation)
  {
    ReservationDTO dto = new ()
    {
      Id = reservation.Id,
      Start = DateTimeUtils.ToUtcString(reservation.Start),
      End = DateTimeUtils.ToUtcString(reservation.End),
      Status = reservation.Status
    };

    return dto;
  }
}
