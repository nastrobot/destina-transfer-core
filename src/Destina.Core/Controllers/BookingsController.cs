using Destina.Core.Models;
using Destina.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;

namespace Destina.Core.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IPricingEngine _pricingEngine;
        private static readonly ConcurrentDictionary<string, BookingRequest> _bookingStore = new();

        public BookingsController(IPricingEngine pricingEngine)
        {
            _pricingEngine = pricingEngine;
        }

        [HttpGet("quote")]
        public IActionResult GetQuote([FromQuery] string pickupZone, [FromQuery] string dropoffZone, [FromQuery] int passengers = 2)
        {
            if (string.IsNullOrWhiteSpace(pickupZone) || string.IsNullOrWhiteSpace(dropoffZone))
            {
                return BadRequest(new { error = "Both pickupZone and dropoffZone parameters are required." });
            }

            var result = _pricingEngine.CalculateQuote(pickupZone, dropoffZone, Math.Max(1, passengers));
            return Ok(result);
        }

        [HttpPost("create")]
        public IActionResult CreateBooking([FromBody] BookingRequest request)
        {
            if (request == null || request.Passenger == null)
            {
                return BadRequest(new { error = "Invalid booking payload." });
            }

            var bookingId = $"DST-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
            request.OfferId = bookingId;

            _bookingStore[bookingId] = request;

            return CreatedAtAction(nameof(GetBookingById), new { id = bookingId }, new
            {
                bookingId,
                status = "CONFIRMED",
                passengerName = $"{request.Passenger.FirstName} {request.Passenger.LastName}",
                flightNumber = request.FlightNumber,
                pickup = request.PickupLocation.Name,
                dropoff = request.DropoffLocation.Name,
                transferDateTime = request.TransferDateTime
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(string id)
        {
            if (_bookingStore.TryGetValue(id, out var booking))
            {
                return Ok(booking);
            }
            return NotFound(new { error = $"Booking with reference '{id}' was not found." });
        }
    }
}
