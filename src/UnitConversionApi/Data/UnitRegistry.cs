using UnitConversionApi.Models;

namespace UnitConversionApi.Data;

/// <summary>
/// Static registry of all supported unit categories and their conversion factors.
///
/// Design notes:
///   - Each category has a "base unit" (factor = 1.0) that all others are expressed relative to.
///   - Linear units use the <see cref="Unit.Factor"/> property: value_in_base = value * factor.
///   - Non-linear units (temperature) use explicit <see cref="Unit.ToBase"/> / <see cref="Unit.FromBase"/> lambdas.
///   - Adding a new unit only requires adding one entry here — no other code changes needed.
/// </summary>
public static class UnitRegistry
{
    private static readonly List<UnitCategory> _categories =
    [
        new UnitCategory
        {
            Id = "length",
            Name = "Length",
            Units =
            [
                new Unit { Id = "meter",      Name = "Meter",      Factor = 1.0 },
                new Unit { Id = "kilometer",  Name = "Kilometer",  Factor = 1_000.0 },
                new Unit { Id = "centimeter", Name = "Centimeter", Factor = 0.01 },
                new Unit { Id = "millimeter", Name = "Millimeter", Factor = 0.001 },
                new Unit { Id = "mile",       Name = "Mile",       Factor = 1_609.344 },
                new Unit { Id = "yard",       Name = "Yard",       Factor = 0.9144 },
                new Unit { Id = "foot",       Name = "Foot",       Factor = 0.3048 },
                new Unit { Id = "inch",       Name = "Inch",       Factor = 0.0254 },
                new Unit { Id = "nautical_mile", Name = "Nautical Mile", Factor = 1_852.0 },
            ]
        },
        new UnitCategory
        {
            Id = "weight",
            Name = "Weight / Mass",
            Units =
            [
                new Unit { Id = "kilogram",   Name = "Kilogram",   Factor = 1.0 },
                new Unit { Id = "gram",       Name = "Gram",       Factor = 0.001 },
                new Unit { Id = "milligram",  Name = "Milligram",  Factor = 0.000_001 },
                new Unit { Id = "metric_ton", Name = "Metric Ton", Factor = 1_000.0 },
                new Unit { Id = "pound",      Name = "Pound",      Factor = 0.453_592_37 },
                new Unit { Id = "ounce",      Name = "Ounce",      Factor = 0.028_349_523_125 },
                new Unit { Id = "stone",      Name = "Stone",      Factor = 6.350_293_18 },
                new Unit { Id = "us_ton",     Name = "US Ton (short ton)", Factor = 907.184_74 },
            ]
        },
        new UnitCategory
        {
            Id = "temperature",
            Name = "Temperature",
            Units =
            [
                // Base unit: Kelvin
                new Unit
                {
                    Id = "kelvin", Name = "Kelvin",
                    ToBase   = v => v,
                    FromBase = v => v
                },
                new Unit
                {
                    Id = "celsius", Name = "Celsius",
                    ToBase   = v => v + 273.15,
                    FromBase = v => v - 273.15
                },
                new Unit
                {
                    Id = "fahrenheit", Name = "Fahrenheit",
                    ToBase   = v => (v + 459.67) * (5.0 / 9.0),
                    FromBase = v => v * (9.0 / 5.0) - 459.67
                },
                new Unit
                {
                    Id = "rankine", Name = "Rankine",
                    ToBase   = v => v * (5.0 / 9.0),
                    FromBase = v => v * (9.0 / 5.0)
                },
            ]
        },
        new UnitCategory
        {
            Id = "volume",
            Name = "Volume",
            Units =
            [
                new Unit { Id = "liter",         Name = "Liter",           Factor = 1.0 },
                new Unit { Id = "milliliter",     Name = "Milliliter",      Factor = 0.001 },
                new Unit { Id = "cubic_meter",    Name = "Cubic Meter",     Factor = 1_000.0 },
                new Unit { Id = "cubic_centimeter", Name = "Cubic Centimeter", Factor = 0.001 },
                new Unit { Id = "gallon_us",      Name = "Gallon (US)",     Factor = 3.785_411_784 },
                new Unit { Id = "gallon_uk",      Name = "Gallon (UK)",     Factor = 4.546_09 },
                new Unit { Id = "fluid_ounce_us", Name = "Fluid Ounce (US)", Factor = 0.029_573_529_5625 },
                new Unit { Id = "cup_us",         Name = "Cup (US)",        Factor = 0.236_588_2365 },
                new Unit { Id = "pint_us",        Name = "Pint (US)",       Factor = 0.473_176_473 },
                new Unit { Id = "quart_us",       Name = "Quart (US)",      Factor = 0.946_352_946 },
            ]
        },
        new UnitCategory
        {
            Id = "speed",
            Name = "Speed",
            Units =
            [
                new Unit { Id = "meter_per_second",    Name = "Meter per Second",    Factor = 1.0 },
                new Unit { Id = "kilometer_per_hour",  Name = "Kilometer per Hour",  Factor = 1.0 / 3.6 },
                new Unit { Id = "mile_per_hour",       Name = "Mile per Hour",       Factor = 0.44704 },
                new Unit { Id = "knot",                Name = "Knot",                Factor = 0.514_444 },
                new Unit { Id = "foot_per_second",     Name = "Foot per Second",     Factor = 0.3048 },
            ]
        },
        new UnitCategory
        {
            Id = "area",
            Name = "Area",
            Units =
            [
                new Unit { Id = "square_meter",      Name = "Square Meter",      Factor = 1.0 },
                new Unit { Id = "square_kilometer",  Name = "Square Kilometer",  Factor = 1_000_000.0 },
                new Unit { Id = "square_foot",       Name = "Square Foot",       Factor = 0.092_903_04 },
                new Unit { Id = "square_yard",       Name = "Square Yard",       Factor = 0.836_127_36 },
                new Unit { Id = "square_mile",       Name = "Square Mile",       Factor = 2_589_988.110_336 },
                new Unit { Id = "hectare",           Name = "Hectare",           Factor = 10_000.0 },
                new Unit { Id = "acre",              Name = "Acre",              Factor = 4_046.856_422_4 },
            ]
        },
        new UnitCategory
        {
            Id = "energy",
            Name = "Energy",
            Units =
            [
                new Unit { Id = "joule",      Name = "Joule",             Factor = 1.0 },
                new Unit { Id = "kilojoule",  Name = "Kilojoule",         Factor = 1_000.0 },
                new Unit { Id = "calorie",    Name = "Calorie",           Factor = 4.184 },
                new Unit { Id = "kilocalorie",Name = "Kilocalorie (kcal)",Factor = 4_184.0 },
                new Unit { Id = "watt_hour",  Name = "Watt-hour",         Factor = 3_600.0 },
                new Unit { Id = "kilowatt_hour", Name = "Kilowatt-hour",  Factor = 3_600_000.0 },
                new Unit { Id = "btu",        Name = "British Thermal Unit (BTU)", Factor = 1_055.05585 },
            ]
        },
    ];

    public static IReadOnlyList<UnitCategory> Categories => _categories.AsReadOnly();

    /// <summary>
    /// Looks up a unit by ID across all categories.
    /// Returns null if not found.
    /// </summary>
    public static (UnitCategory? category, Unit? unit) FindUnit(string unitId)
    {
        var id = unitId.Trim().ToLowerInvariant();
        foreach (var cat in _categories)
        {
            var unit = cat.Units.FirstOrDefault(u => u.Id == id);
            if (unit is not null)
                return (cat, unit);
        }
        return (null, null);
    }
}
