using FlightInformation.Contexts;
using FlightInformation.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightInformation.Repositories
{
    public class FlightService : IFlightService
    {
        private readonly FlightDbContext _context;

        public FlightService(FlightDbContext context)
        {
            _context = context;
        }

        public async Task<List<Flight>> GetAllFlights() => await _context.Flights.ToListAsync();

        public async Task<Flight?> GetFlightById(int iFlightId) => await _context.Flights.FindAsync(iFlightId);

        public async Task<Flight> CreateFlightInfo(Flight oFlight)
        {
            _context.Flights.Add(oFlight);
            await _context.SaveChangesAsync();
            return oFlight;
        }

        public async Task<bool> UpdateFlightInfo(int iFlightId, Flight oFlight)
        {
            var oExistingFlight = await _context.Flights.FindAsync(iFlightId);
            if (oExistingFlight == null) return false;

            oExistingFlight.FlightNumber = oFlight.FlightNumber;
            oExistingFlight.Airline = oFlight.Airline;
            oExistingFlight.DepartureAirport = oFlight.DepartureAirport;
            oExistingFlight.ArrivalAirport = oFlight.ArrivalAirport;
            oExistingFlight.DepartureTime = oFlight.DepartureTime;
            oExistingFlight.ArrivalTime = oFlight.ArrivalTime;
            oExistingFlight.Status = oFlight.Status;
            oExistingFlight.Updated = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFlightInfo(int iFlightId)
        {
            var oFlight = await _context.Flights.FindAsync(iFlightId);
            if (oFlight == null) return false;
            _context.Flights.Remove(oFlight);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Flight>> SearchFlight(string? sAirline, string? sDepartureAirport, string? sArrivalAirport, DateTime? dtFrom, DateTime? dtTo)
        {
            var oFlightQuery = _context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(sAirline))
                oFlightQuery = oFlightQuery.Where(f => f.Airline.Contains(sAirline));

            if (!string.IsNullOrEmpty(sDepartureAirport))
                oFlightQuery = oFlightQuery.Where(f => f.DepartureAirport.Contains(sDepartureAirport));

            if (!string.IsNullOrEmpty(sArrivalAirport))
                oFlightQuery = oFlightQuery.Where(f => f.ArrivalAirport.Contains(sArrivalAirport));

            if (dtFrom.HasValue)
                oFlightQuery = oFlightQuery.Where(f => f.DepartureTime >= dtFrom);

            if (dtTo.HasValue)
                oFlightQuery = oFlightQuery.Where(f => f.ArrivalTime <= dtTo);

            return await oFlightQuery.ToListAsync();
        }
    }
}
