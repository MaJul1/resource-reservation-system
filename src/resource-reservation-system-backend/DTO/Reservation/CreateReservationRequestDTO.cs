using System;

namespace resource_reservation_system_backend.DTO.Reservation;

public class CreateReservationRequestDTO
{
  public DateTime Start {get; set;}
  public DateTime End {get; set;}
  public int ResourceId {get; set;}
  public string FirstName {get; set;} = null!;
  public string LastName {get; set;} = null!;
  public string PhoneNumber {get; set;} = null!;
  public string Email {get; set;} = null!;
}
