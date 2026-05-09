using System;

namespace resource_reservation_system_backend.Models;

public class ItemAllocation
{
  public int Id {get; set;}
  public int InventoryId {get; set;}
  public string Name {get; set;} = null!;
  public int Quantity {get; set;}

  public int ReservationId {get; set;}
  public Reservation Reservation {get; set;} = null!;
}
