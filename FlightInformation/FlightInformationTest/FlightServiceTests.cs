using FlightInformation.Contexts;
using FlightInformation.Models;
using FlightInformation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FlightInformationTest
{
    public class FlightServiceTests
    {
        private readonly IFlightService _service;
        private readonly FlightDbContext _context;

        public FlightServiceTests()
        {
            var options = new DbContextOptionsBuilder<FlightDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new FlightDbContext(options);
            _service = new FlightService(_context);
        }

        [Fact]
        public async Task CanCreateAndGetFlight()
        {
            var oFlight = new Flight
            {
                FlightNumber = "JE150",
                Airline = "Jetstar",
                DepartureAirport = "WLG",
                ArrivalAirport = "SIN",
                DepartureTime = new DateTime(2025, 6, 30, 15, 0, 0),
                ArrivalTime = new DateTime(2025, 7, 1, 2, 0, 0),
                Status = FlightStatus.InAir,
                Created = DateTime.Now
            };

            var oCreatedFlight = await _service.CreateFlightInfo(oFlight);
            var oReceivedFlight = await _service.GetFlightById(oCreatedFlight.Id);

            Assert.NotNull(oReceivedFlight);
            Assert.Equal("JE150", oReceivedFlight!.FlightNumber);
        }

        [Fact]
        public async Task CreateAsync_InvalidFlight_ShouldThrowValidationException()
        {
            var oFlight = new Flight(); // Missing required fields
            await Assert.ThrowsAsync<DbUpdateException>(() => _service.CreateFlightInfo(oFlight));
        }

        [Fact]
        public async Task SearchAsync_NoMatches_ReturnsEmptyList()
        {
            var oResults = await _service.SearchFlight("InvalidAirline", null, null, null, null);
            Assert.Empty(oResults);
        }

        [Fact]
        public async Task SearchAsync_TimeRangeFilter_ReturnsExpectedFlights()
        {
            var dtFrom = DateTime.UtcNow;
            var dtTo = DateTime.UtcNow.AddHours(2);
            var oFlightList = await _service.SearchFlight(null, null, null, dtFrom, dtTo);
            Assert.All(oFlightList, f => Assert.InRange(f.DepartureTime, dtFrom, dtTo));
        }

        [Fact]
        public async Task Update_NonExistentFlight_ReturnsNotFound()
        {
            var result = await _service.UpdateFlightInfo(999, new Flight { Id = 999 });
            Assert.IsType<NotFoundResult>(result);
        }

    }
}