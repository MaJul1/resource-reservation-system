using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace resource_reservation_system_backend.DTO.User;

public class UserDTO
{
  public int Id {get; set;}
  public string FirstName {get; set;} = null!;
  public string LastName {get; set;} = null!;
  public string PhoneNumber {get; set;} = null!;
  public string Email {get; set;} = null!;
}
