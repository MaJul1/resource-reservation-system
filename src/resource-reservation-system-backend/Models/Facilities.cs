using System;

namespace resource_reservation_system_backend.Models;

public class Facility
{
  public int Id {get; set;}
  public string Name {get; set;} = null!;
  public string Type {get; set;} = null!;
  public string Description {get; set;} = null!;
  public string Location {get; set;} = null!;
  public int Capacity {get; set;}

  public ICollection<Reservation> Reservations {get; set;} = [];
  public ICollection<Department> Departments {get; set;} = [];
  public ICollection<ItemAllocation> ItemsAllocated {get; set;} = [];

}
