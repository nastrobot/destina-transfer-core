using Destina.Core.Models;
using System;
using System.Collections.Generic;

namespace Destina.Core.Services
{
    public interface IPricingEngine
    {
        QuoteResult CalculateQuote(string pickupZone, string dropoffZone, int passengerCount);
        double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2);
    }

    public class PricingEngine : IPricingEngine
    {
        private const decimal EurToTryRate = 38.50m;

        // Fixed baseline transfer distance matrix (Antalya Airport regional hubs)
        private readonly Dictionary<string, (double DistanceKm, int DurationMin)> _zoneMatrix = new(StringComparer.OrdinalIgnoreCase)
        {
            { "AYT-KUNDU", (15.0, 20) },
            { "AYT-BELEK", (35.0, 35) },
            { "AYT-SIDE", (65.0, 55) },
            { "AYT-ALANYA", (125.0, 110) },
            { "AYT-KEMER", (60.0, 50) },
            { "AYT-TEKIROVA", (75.0, 65) },
            { "AYT-KAS", (200.0, 190) }
        };

        private readonly List<VehicleCategory> _fleetCategories = new()
        {
            new VehicleCategory { Id = 1, CategoryName = "Mercedes Vito VIP", ModelYear = "2023-2024", MaxPassengers = 6, MaxLuggage = 6, BaseRateEur = 25m, RatePerKmEur = 0.85m },
            new VehicleCategory { Id = 2, CategoryName = "Mercedes Sprinter VIP", ModelYear = "2023", MaxPassengers = 14, MaxLuggage = 14, BaseRateEur = 40m, RatePerKmEur = 1.10m },
            new VehicleCategory { Id = 3, CategoryName = "Maybach Design VIP Lounge", ModelYear = "2024", MaxPassengers = 4, MaxLuggage = 4, BaseRateEur = 60m, RatePerKmEur = 1.60m }
        };

        public QuoteResult CalculateQuote(string pickupZone, string dropoffZone, int passengerCount)
        {
            var routeKey = $"{pickupZone}-{dropoffZone}".ToUpperInvariant();
            var reverseKey = $"{dropoffZone}-{pickupZone}".ToUpperInvariant();

            double distanceKm = 40.0;
            int durationMin = 40;

            if (_zoneMatrix.TryGetValue(routeKey, out var metrics))
            {
                distanceKm = metrics.DistanceKm;
                durationMin = metrics.DurationMin;
            }
            else if (_zoneMatrix.TryGetValue(reverseKey, out metrics))
            {
                distanceKm = metrics.DistanceKm;
                durationMin = metrics.DurationMin;
            }

            var quotes = new List<VehicleQuote>();
            foreach (var vehicle in _fleetCategories)
            {
                if (passengerCount > vehicle.MaxPassengers) continue;

                var priceEur = Math.Round(vehicle.BaseRateEur + (decimal)distanceKm * vehicle.RatePerKmEur, 2);
                var priceTry = Math.Round(priceEur * EurToTryRate, 2);

                quotes.Add(new VehicleQuote
                {
                    VehicleName = vehicle.CategoryName,
                    MaxPax = vehicle.MaxPassengers,
                    PriceEur = priceEur,
                    PriceTry = priceTry
                });
            }

            var bestPriceEur = quotes.Count > 0 ? quotes[0].PriceEur : 0m;
            return new QuoteResult
            {
                RouteDescription = $"{pickupZone} -> {dropoffZone}",
                EstimatedDistanceKm = distanceKm,
                EstimatedDurationMinutes = durationMin,
                TotalPriceEur = bestPriceEur,
                TotalPriceTry = Math.Round(bestPriceEur * EurToTryRate, 2),
                AvailableVehicles = quotes
            };
        }

        public double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return Math.Round(R * c, 2);
        }
    }
}
