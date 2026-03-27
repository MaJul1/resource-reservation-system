using System;
using Microsoft.AspNetCore.StaticAssets;

namespace resource_reservation_system_backend.Utils;

public class DateTimeUtils
{
  public static string ToUtcString(DateTime dateTime)
  {
    return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
  }

  public static int GetMinutesDifference(DateTime start, DateTime end)
  {
    return (int)(end - start).TotalMinutes;
  }
}
