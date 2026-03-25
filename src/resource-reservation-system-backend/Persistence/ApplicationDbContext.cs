using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Persistence;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext (DbContextOptions<ApplicationDbContext> builder) : base (builder)
  { }

  public DbSet<Reservation> Reservations {get; set;}
  public DbSet<Resource> Resources {get; set;}
  public DbSet<User> Users {get; set;}

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Reservation>()
      .HasOne(e => e.User)
      .WithOne(e => e.Reservation)
      .HasForeignKey<Reservation>(e => e.UserId);
  }
}
