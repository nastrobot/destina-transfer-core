# destina-transfer-core

Backend booking engine and transfer dispatch API module powering regional airport transfer operations for [destinatourism.com](https://destinatourism.com).

Built with **C#** and **ASP.NET Core 8**, this service handles automated transfer quoting across Antalya regional hubs, passenger booking payloads, vehicle category matching, and fleet dispatch status.

---

## Architecture & Responsibilities

- **Dynamic Quoting Engine:** Distance and zone-matrix calculation between regional transit hubs (Antalya Airport `AYT`, Kundu, Belek, Side, Alanya, Kemer) with multi-currency conversion (`EUR` / `TRY`).
- **Booking Lifecycle:** Handles B2B and direct consumer booking payloads including flight monitoring fields, child seat requirements, terminal nameplate requests, and upsell addons.
- **Fleet Allocation:** Vehicle capacity checks across Mercedes Vito VIP (6 pax), Sprinter (14 pax), and luxury VIP Maybach design categories.

---

## API Endpoints

### 1. Calculate Transfer Quote
```http
GET /api/v1/bookings/quote?pickupZone=AYT&dropoffZone=BELEK&passengers=4
```

**Response (200 OK):**
```json
{
  "routeDescription": "AYT -> BELEK",
  "estimatedDistanceKm": 35.0,
  "estimatedDurationMinutes": 35,
  "totalPriceEur": 54.75,
  "totalPriceTry": 2107.88,
  "availableVehicles": [
    {
      "vehicleName": "Mercedes Vito VIP",
      "maxPax": 6,
      "priceEur": 54.75,
      "priceTry": 2107.88
    },
    {
      "vehicleName": "Mercedes Sprinter VIP",
      "maxPax": 14,
      "priceEur": 78.50,
      "priceTry": 3022.25
    }
  ]
}
```

### 2. Create Booking
```http
POST /api/v1/bookings/create
Content-Type: application/json
```

**Request Body:**
```json
{
  "passengers": 3,
  "luggagePlaces": 3,
  "flightNumber": "TK2410",
  "shieldText": "MR. SMITH",
  "transferDateTime": "2026-10-15T14:30:00Z",
  "passenger": {
    "firstName": "John",
    "lastName": "Smith",
    "phone": "+447911123456",
    "email": "john.smith@example.co.uk"
  },
  "pickupLocation": {
    "name": "Antalya Airport Terminal 2",
    "zoneCode": "AYT"
  },
  "dropoffLocation": {
    "name": "Rixos Premium Belek",
    "zoneCode": "BELEK"
  }
}
```

### 3. Fleet Status
```http
GET /api/v1/fleet/vehicles
```

---

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Local Development
```bash
git clone https://github.com/nastrobot/destina-transfer-core.git
cd destina-transfer-core/src/Destina.Core
dotnet restore
dotnet run
```

The API will listen at `https://localhost:5001` or `http://localhost:5000`.

---

## License
Proprietary — Internal core service for Destina Tourism operations.
