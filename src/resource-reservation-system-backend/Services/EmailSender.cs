using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Resend;

namespace resource_reservation_system_backend.Services;

public class EmailSender : IEmailSender
{
  private readonly IConfiguration _config;
  public EmailSender(IConfiguration config)
  {
    _config = config;
  }

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    IResend resend = ResendClient.Create(_config["ResendToken"]!);

    await resend.EmailSendAsync( new EmailMessage()
    {
        From = "onboarding@resend.dev",
        To = email,
        Subject = subject,
        HtmlBody = htmlMessage,
    } );
  }
}
