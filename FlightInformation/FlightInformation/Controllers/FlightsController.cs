using FlightInformation.Models;
using FlightInformation.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlightInformation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _service;
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(IFlightService service, ILogger<FlightsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var oFlights = await _service.GetAllFlights();
            return Ok(oFlights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int iFlightId)
        {
            var oFlight = await _service.GetFlightById(iFlightId);
            if (oFlight == null) return NotFound();
            return Ok(oFlight);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Flight oFlight)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var oCreatedFlight = await _service.CreateFlightInfo(oFlight);
            return CreatedAtAction(nameof(GetById), new { id = oCreatedFlight.Id }, oCreatedFlight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int iFlightId, [FromBody] Flight oFlight)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var bSuccess = await _service.UpdateFlightInfo(iFlightId, oFlight);
            if (!bSuccess) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int iFlightId)
        {
            var bSuccess = await _service.DeleteFlightInfo(iFlightId);
            if (!bSuccess) return NotFound();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
                                                    [FromQuery] string? sAirline,
                                                    [FromQuery] string? sDepartureAirport,
                                                    [FromQuery] string? sArrivalAirport,
                                                    [FromQuery] DateTime? dtFrom,
                                                    [FromQuery] DateTime? dtTo
                                                )
        {
            var oResult = await _service.SearchFlight(sAirline, sDepartureAirport, sArrivalAirport, dtFrom, dtTo);
            return Ok(oResult);
        }
    }
}
