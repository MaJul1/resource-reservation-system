using System;

namespace resource_reservation_system_backend.Models;

public class Resource
{
  public int Id {get; set;}
  public string Name {get; set;} = null!;
  public string Type {get; set;} = null!;
  public string Description {get; set;} = null!;

  public int ReservationId {get; set;}
  public Reservation Reservation {get; set;} = null!;
}
