using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Persistence;

public class AppDbContext : DbContext
{
  public AppDbContext (DbContextOptions<AppDbContext> builder) : base (builder)
  { }

  public DbSet<Reservation> Reservations {get; set;}
  public DbSet<Facility> Facilities {get; set;}
  public DbSet<User> Users {get; set;}
  public DbSet<Department> Departments {get; set;}
  public DbSet<ItemAllocation> ItemAllocations {get; set;}

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Reservation>()
      .HasOne(e => e.User)
      .WithOne(e => e.Reservation)
      .HasForeignKey<Reservation>(e => e.UserId);
  }
}
