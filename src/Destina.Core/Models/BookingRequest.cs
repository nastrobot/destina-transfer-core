using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Destina.Core.Models
{
    public class BookingRequest
    {
        public string? OfferId { get; set; }
        public int Passengers { get; set; }
        public int LuggagePlaces { get; set; }
        public int SportLuggagePlaces { get; set; }
        public int WheelchairsPlaces { get; set; }
        public int ChildSeats { get; set; }
        public string? FlightNumber { get; set; }
        public string? ShieldText { get; set; } // Terminal greeting nameplate
        public string? Comment { get; set; }
        public DateTime TransferDateTime { get; set; }

        public required MainPassenger Passenger { get; set; }
        public required TransferLocation PickupLocation { get; set; }
        public required TransferLocation DropoffLocation { get; set; }
        public List<UpsellItem> Upsells { get; set; } = new();
    }

    public class MainPassenger
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class TransferLocation
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ZoneCode { get; set; } // e.g., AYT, KUNDU, BELEK, SIDE, ALANYA, KEMER
    }

    public class UpsellItem
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class VehicleCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty; // Mercedes Vito VIP, Sprinter, Maybach VIP
        public string ModelYear { get; set; } = string.Empty;
        public int MaxPassengers { get; set; }
        public int MaxLuggage { get; set; }
        public string? ImageUrl { get; set; }
        public decimal BaseRateEur { get; set; }
        public decimal RatePerKmEur { get; set; }
    }

    public class QuoteResult
    {
        public string RouteDescription { get; set; } = string.Empty;
        public double EstimatedDistanceKm { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public decimal TotalPriceEur { get; set; }
        public decimal TotalPriceTry { get; set; }
        public List<VehicleQuote> AvailableVehicles { get; set; } = new();
    }

    public class VehicleQuote
    {
        public string VehicleName { get; set; } = string.Empty;
        public int MaxPax { get; set; }
        public decimal PriceEur { get; set; }
        public decimal PriceTry { get; set; }
    }
}
