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
      Status = reservation.Status,
      User = reservation.User.ToUserDTO()
    };

    return dto;
  }

  public static Reservation ToReservation(this CreateReservationRequestDTO dto)
  {
    Reservation reservation = new ()
    {
      Start = dto.Start,
      End = dto.End,
      Status = Enums.Status.PENDING,
      User = new User()
      {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber
      }
    };

    return reservation;
  }
}
