using System;

namespace resource_reservation_system_backend.DTO;

public class ReservationDTO
{
  public int Id {get; set;}
  public string Start {get; set;} = null!;
  public string End {get; set;} = null!;
  public string Status {get; set;} = null!;
}
