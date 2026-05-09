using System;

namespace resource_reservation_system_backend.Models;

public class Department
{
  public int Id {get; set;}
  public string Name {get; set;} = null!;
  public ICollection<Facility> Facilities {get; set;} = [];
}
