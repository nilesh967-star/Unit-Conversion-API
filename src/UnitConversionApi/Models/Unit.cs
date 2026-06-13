namespace UnitConversionApi.Models;

/// <summary>
/// Represents a single unit of measurement within a category.
/// </summary>
public class Unit
{
    /// <summary>
    /// Unique identifier / symbol for the unit (e.g. "m", "kg", "celsius").
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Human-readable display name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Multiplier to convert this unit into the category's base unit.
    /// For temperature, this field is ignored — use <see cref="ToBase"/> / <see cref="FromBase"/> instead.
    /// </summary>
    public double? Factor { get; init; }

    /// <summary>Optional formula to convert value → base unit (used for non-linear scales like temperature).</summary>
    public Func<double, double>? ToBase { get; init; }

    /// <summary>Optional formula to convert base unit value → this unit.</summary>
    public Func<double, double>? FromBase { get; init; }
}
