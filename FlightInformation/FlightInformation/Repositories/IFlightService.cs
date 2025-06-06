using FlightInformation.Models;

namespace FlightInformation.Repositories
{
    public interface IFlightService
    {
        Task<List<Flight>> GetAllFlights();
        Task<Flight?> GetFlightById(int iFlightId);
        Task<Flight> CreateFlightInfo(Flight oFlight);
        Task<bool> UpdateFlightInfo(int iFlightId, Flight oUpdatedFlight);
        Task<bool> DeleteFlightInfo(int iFlightId);
        Task<List<Flight>> SearchFlight(string? airline, string? departureAirport, string? arrivalAirport, DateTime? from, DateTime? to);
    }
}
