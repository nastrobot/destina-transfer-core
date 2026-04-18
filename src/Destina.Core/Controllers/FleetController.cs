using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Destina.Core.Controllers
{
    [ApiController]
    [Route("api/v1/fleet")]
    public class FleetController : ControllerBase
    {
        [HttpGet("vehicles")]
        public IActionResult GetFleetStatus()
        {
            var fleet = new List<object>
            {
                new { id = "DST-07-01", model = "Mercedes-Benz Vito Tourer VIP", plate = "07 DST 104", status = "AVAILABLE", hub = "Antalya Airport (AYT)" },
                new { id = "DST-07-02", model = "Mercedes-Benz Vito Extra Long", plate = "07 DST 218", status = "ON_TRIP", destination = "Belek Gloria Serenity" },
                new { id = "DST-07-03", model = "Mercedes-Benz Sprinter VIP 16+1", plate = "07 DST 409", status = "AVAILABLE", hub = "Alanya Center" },
                new { id = "DST-07-04", model = "Maybach Lounge Edition Vito", plate = "07 VIP 991", status = "MAINTENANCE", notes = "Scheduled servicing" }
            };

            return Ok(new { timestamp = System.DateTime.UtcNow, fleetCount = fleet.Count, fleet });
        }
    }
}
