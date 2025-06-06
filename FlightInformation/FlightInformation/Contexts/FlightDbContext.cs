using FlightInformation.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightInformation.Contexts
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        public DbSet<Flight> Flights => Set<Flight>();
    }
}
