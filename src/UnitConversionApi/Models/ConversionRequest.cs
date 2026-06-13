using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

/// <summary>Request body for a unit conversion.</summary>
public class ConversionRequest
{
    /// <summary>The value to convert.</summary>
    [Required]
    public double Value { get; init; }

    /// <summary>The unit to convert from (e.g. "meter", "celsius", "kilogram").</summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FromUnit { get; init; } = string.Empty;

    /// <summary>The unit to convert to (e.g. "foot", "fahrenheit", "pound").</summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string ToUnit { get; init; } = string.Empty;
}
