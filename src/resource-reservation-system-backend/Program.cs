using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using resource_reservation_system_backend.Background;
using resource_reservation_system_backend.Interfaces;
using resource_reservation_system_backend.Models;
using resource_reservation_system_backend.Persistence;
using resource_reservation_system_backend.Services;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddHostedService<StatusUpdateBackgroundService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "swagger");
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Unhandled exception occurred.");

        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            message = statusCode == 500 
                ? "An unexpected error occurred." 
                : exception?.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.MapControllers();


app.Run();
