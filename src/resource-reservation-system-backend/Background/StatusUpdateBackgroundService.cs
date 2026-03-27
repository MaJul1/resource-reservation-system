using System;
using resource_reservation_system_backend.Enums;
using resource_reservation_system_backend.Persistence;

namespace resource_reservation_system_backend.Background;

public partial class StatusUpdateBackgroundService : BackgroundService
{

  private readonly IServiceScopeFactory _scopeFactory;
  private readonly ILogger<StatusUpdateBackgroundService> _logger;

  public StatusUpdateBackgroundService(
      IServiceScopeFactory scopeFactory,
      ILogger<StatusUpdateBackgroundService> logger)
  {
    _scopeFactory = scopeFactory;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Update background service started.");

    while (!stoppingToken.IsCancellationRequested)
    {
      using var scope = _scopeFactory.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      var utcDateTimeNow = DateTime.Now.ToUniversalTime();
      var pendingAndApprovedStatus = new [] {Enums.Status.APPROVED, Enums.Status.PENDING};

      var reservations = context.Reservations
        .Where(r => r.Start <= utcDateTimeNow)
        .Where(r => pendingAndApprovedStatus.Contains(r.Status));
      
      if (reservations.Any())
      {
        foreach (var r in reservations)
        {
          if (r.Status == Enums.Status.APPROVED)
          {
            r.Status = Enums.Status.ONGOING;
            LogStatus(r.Id, "ongoing");
          }
          else if (r.Status == Enums.Status.PENDING)
          {
            r.Status = Enums.Status.DENIED;
            LogStatus(r.Id, "denied");
          }
        }

        await context.SaveChangesAsync(stoppingToken);
      }

      var ongoingReservation = context.Reservations
          .Where(r => r.Status == Enums.Status.ONGOING)
          .Where(r => r.End <= utcDateTimeNow);

      if (ongoingReservation.Any())
      {
        foreach (var r in ongoingReservation)
        {
          r.Status = Enums.Status.DONE;
          LogStatus(r.Id, "done");
        }

        await context.SaveChangesAsync(stoppingToken);
      }

      await Task.Delay(60000, stoppingToken);
    }
  }


    [LoggerMessage(Level = LogLevel.Information, Message = "Reservation with an Id of {id} is now {status}.")]
    private partial void LogStatus(int id, string status);
}
