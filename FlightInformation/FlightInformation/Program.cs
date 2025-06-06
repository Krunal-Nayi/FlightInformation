using FlightInformation.Contexts;
using FlightInformation.Models;
using FlightInformation.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<FlightDbContext>(opt => opt.UseInMemoryDatabase("FlightsDb"));
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Flight Information API",
        Version = "v1",
        Description = "An API to manage flight information.",
        Contact = new OpenApiContact
        {
            Name = "Krunal Nayi",
            Email = "kunalnayi@outlook.com"
        }
    });
});
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight Information API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Seed dummy data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    db.Flights.AddRange(
        new Flight { Id = 1, FlightNumber = "AI101", Airline = "Air New Zealand", DepartureAirport = "CHC", ArrivalAirport = "SYD", DepartureTime = new DateTime(2025, 06, 24, 23, 00, 00), ArrivalTime = new DateTime(2025, 06, 25, 11, 00, 00), Status = FlightStatus.Cancelled, Created = DateTime.Now },
        new Flight { Id = 2, FlightNumber = "VI102", Airline = "Virgin Australia", DepartureAirport = "AKL", ArrivalAirport = "DXB", DepartureTime = new DateTime(2025, 06, 20, 11, 00, 00), ArrivalTime = new DateTime(2025, 06, 20, 17, 00, 00), Status = FlightStatus.Delayed, Created = DateTime.Now },
        new Flight { Id = 3, FlightNumber = "QA103", Airline = "Qantas", DepartureAirport = "NPE", ArrivalAirport = "FJI", DepartureTime = new DateTime(2025, 06, 18, 05, 00, 00), ArrivalTime = new DateTime(2025, 06, 18, 11, 00, 00), Status = FlightStatus.Landed, Created = DateTime.Now }
    );
    db.SaveChanges();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();