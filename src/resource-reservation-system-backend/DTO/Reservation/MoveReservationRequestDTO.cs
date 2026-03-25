using System;

namespace resource_reservation_system_backend.DTO.Reservation;

public class MoveReservationRequestDTO
{
  public int Id {get; set;}
  public DateTime NewStart {get; set;}
  public DateTime NewEnd {get; set;}
}
