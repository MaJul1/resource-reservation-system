using System;

namespace resource_reservation_system_backend.Utils;

public class DateTimeUtils
{
  public static string ToUtcString(DateTime dateTime)
  {
    return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
  }
}
