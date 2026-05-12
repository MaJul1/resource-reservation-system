using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Utils;

namespace resource_reservation_system_backend.Mapper;

public static class ReservationMapper
{
  public static DetailedReservationDTO ToReservationDTO(this Reservation reservation)
  {
    DetailedReservationDTO dto = new ()
    {
      Id = reservation.Id,
      Start = DateTimeUtils.ToUtcString(reservation.Start),
      End = DateTimeUtils.ToUtcString(reservation.End),
      Status = reservation.Status,
      Facility = reservation.Resource.ToSummarizedFacilityDTO(),
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
      ResourceId = dto.ResourceId,
      Purpose = dto.Purpose,
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

  public static SummaryReservationDTO ToSummaryReservationDTO(this Reservation reservation)
  {
    SummaryReservationDTO dto = new ()
    {
      Id = reservation.Id,
      Start = DateTimeUtils.ToUtcString(reservation.Start),
      End = DateTimeUtils.ToUtcString(reservation.End),
      Purpose = reservation.Purpose,
      Status = reservation.Status,
      ResourceName = reservation.Resource.Name
    };

    return dto;
  }
}
