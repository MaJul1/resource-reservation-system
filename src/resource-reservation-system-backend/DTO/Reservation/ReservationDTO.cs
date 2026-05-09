using System;
using resource_reservation_system_backend.DTO.Resource;
using resource_reservation_system_backend.DTO.User;
using resource_reservation_system_backend.Enums;

namespace resource_reservation_system_backend.DTO.Reservation;

public class ReservationDTO
{
  public int Id {get; set;}
  public string Start {get; set;} = null!;
  public string End {get; set;} = null!;
  public string Purppose {get; set;} = null!;
  public Status Status {get; set;}
  public ResourceDTO Resource {get; set;} = null!;
  public UserDTO User {get; set;} = null!;
}
