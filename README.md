# Unit Conversion API

A RESTful ASP.NET Core Web API with a React frontend for converting numerical values between units of measurement across multiple categories.

---

## Supported Categories

| Category    | Example units                                      |
|-------------|----------------------------------------------------|
| Length      | meter, kilometer, mile, foot, inch, yard, …        |
| Weight/Mass | kilogram, gram, pound, ounce, stone, metric ton, … |
| Temperature | celsius, fahrenheit, kelvin, rankine               |
| Volume      | liter, gallon_us, fluid_ounce_us, cup_us, …        |
| Speed       | meter_per_second, kilometer_per_hour, knot, …      |
| Area        | square_meter, square_kilometer, hectare, acre, …   |
| Energy      | joule, kilocalorie, watt_hour, kilowatt_hour, …    |

---

## Prerequisites

| Tool | Version | Download |
|------|---------|----------|
| .NET SDK | 10.x | https://dotnet.microsoft.com/en-us/download/dotnet/10.0 |
| Node.js | 18+ | https://nodejs.org |

Verify both are installed:

```bash
dotnet --version   # 10.x.x
node --version     # v18.x or higher
```

---

## Project Structure

```
UnitConversionApi/
├── src/
│   └── UnitConversionApi/        # ASP.NET Core Web API
├── tests/
│   └── UnitConversionApi.Tests/  # xUnit unit + integration tests
├── frontend/                     # React + TypeScript (Vite)
└── UnitConversionApi.sln
```

---

## Running Locally

You need **two terminals** open at the same time — one for the backend, one for the frontend.

### Step 1 — Clone the repository

```bash
git clone <your-repo-url>
cd UnitConversionApi
```

### Step 2 — Start the Backend (API)

Open **Terminal 1** and run:

```bash
dotnet run --project src/UnitConversionApi
```

The API will start at:
- **http://localhost:5000** — API base URL
- **http://localhost:5000** — Swagger UI (interactive docs, opens at root)

> You can test endpoints directly from the Swagger UI in your browser.

### Step 3 — Start the Frontend

Open **Terminal 2** and run:

```bash
cd frontend
npm install
npm run dev
```

The frontend will start at:
- **http://localhost:5173** — React UI

Open **http://localhost:5173** in your browser to use the app.

> The frontend proxies all `/api` requests to the backend automatically, so both must be running at the same time.

---

## Running Tests

From the root `UnitConversionApi/` folder:

```bash
dotnet test
```

This runs both unit tests and integration tests.

---

## API Reference

### `POST /api/conversion`

Convert a value from one unit to another.

**Request body**

```json
{
  "value": 100,
  "fromUnit": "kilometer",
  "toUnit": "mile"
}
```

**Response `200 OK`**

```json
{
  "inputValue": 100,
  "fromUnit": "kilometer",
  "fromUnitName": "Kilometer",
  "outputValue": 62.13711922369394,
  "toUnit": "mile",
  "toUnitName": "Mile",
  "category": "Length"
}
```

**Response `400 Bad Request`** — unknown unit ID or incompatible categories.

---

### `GET /api/conversion/units`

Returns all supported categories and their units.

---

### `GET /api/conversion/units/{categoryId}`

Returns units for a specific category (e.g. `length`, `temperature`).

**Response `404 Not Found`** — category does not exist.

---

## Exploring with Swagger UI

With the backend running, open **http://localhost:5000** in your browser. Swagger UI loads at the root and lets you try every endpoint interactively without any additional tooling.

---

## Design Decisions & Trade-offs

### Single registry, no database
All units and conversion factors live in `UnitRegistry.cs`. This keeps the project self-contained while satisfying the hardcoded-data requirement. Migrating to a database later only requires swapping `UnitRegistry` for a repository backed by EF Core — no controller or service changes needed.

### Base-unit conversion strategy
Each category has one implicit base unit (e.g. meter for length, kelvin for temperature). Every conversion goes A → base → B. Adding a new unit only requires one number (its factor relative to base) — no N×N conversion table.

### Linear vs non-linear units
Most units use a simple multiplication factor. Temperature uses explicit `ToBase` / `FromBase` lambdas to handle offset-based scales (Celsius, Fahrenheit). The same pattern generalises to any future non-linear scale.

### Interface-driven service layer
`IConversionService` is the only dependency the controller takes. Swapping the implementation (e.g. database-backed) requires no controller changes.

### Unit IDs are lowercase strings
Stable string IDs (e.g. `"kilometer"`, `"gallon_us"`) instead of enums make the API easy to extend without recompilation or client-breaking changes.
