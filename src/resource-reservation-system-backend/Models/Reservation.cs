using System;
using resource_reservation_system_backend.Enums;

namespace resource_reservation_system_backend.Models;

public class Reservation
{
  public int Id {get; set;}
  public DateTime Start {get; set;}
  public DateTime End {get; set;}
  public Status Status {get; set;}

  public int UserId {get ;set;}
  public User User {get; set;} = null!;
  public int ResourceId {get; set;}
  public Resource Resource {get; set;} = null!;
}
