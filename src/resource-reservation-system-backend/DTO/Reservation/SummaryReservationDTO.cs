using System;
using resource_reservation_system_backend.Enums;

namespace resource_reservation_system_backend.DTO.Reservation;

public class SummaryReservationDTO
{
  public int Id { get; set; }
  public string Start { get; set; } = null!;
  public string End { get; set; } = null!;
  public string Purpose { get; set; } = null!;
  public Status Status { get; set; }
  public string ResourceName { get; set; } = null!;
}
