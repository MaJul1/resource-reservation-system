using System;

namespace resource_reservation_system_backend.Models;

public class User
{
  public int Id {get; set;}
  public string FirstName {get; set;} = null!;
  public string LastName {get; set;} = null!;
  public string PhoneNumber {get; set;} = null!;
  public string Email {get; set;} = null!;

  public Reservation Reservation {get; set;} = null!;
}
