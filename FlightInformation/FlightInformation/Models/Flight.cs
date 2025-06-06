using System.ComponentModel.DataAnnotations;

namespace FlightInformation.Models
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string Airline { get; set; }
        [Required]
        public string DepartureAirport { get; set; }
        [Required]
        public string ArrivalAirport { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        [Required]
        public FlightStatus Status { get; set; }

        [Required]
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum FlightStatus
    {
        Scheduled,
        Delayed,
        Cancelled,
        InAir,
        Landed
    }
}
