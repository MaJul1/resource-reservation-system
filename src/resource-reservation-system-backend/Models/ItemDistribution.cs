using System;

namespace resource_reservation_system_backend.Models;

public class ItemDistribution
{
  public int Id {get; set;}
  public int ItemId {get; set;}
  public string Name {get; set;} = null!;
  public int Quantity {get; set;}
}
