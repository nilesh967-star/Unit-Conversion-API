using UnitConversionApi.Data;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <inheritdoc />
public class ConversionService : IConversionService
{
    /// <inheritdoc />
    public ConversionResult Convert(double value, string fromUnitId, string toUnitId)
    {
        var (fromCategory, fromUnit) = UnitRegistry.FindUnit(fromUnitId);
        if (fromUnit is null)
            throw new ArgumentException($"Unknown unit: '{fromUnitId}'.", nameof(fromUnitId));

        var (toCategory, toUnit) = UnitRegistry.FindUnit(toUnitId);
        if (toUnit is null)
            throw new ArgumentException($"Unknown unit: '{toUnitId}'.", nameof(toUnitId));

        if (fromCategory!.Id != toCategory!.Id)
            throw new ArgumentException(
                $"Cannot convert between different categories: '{fromCategory.Name}' and '{toCategory.Name}'.");

        var baseValue = ToBaseUnit(value, fromUnit);
        var outputValue = FromBaseUnit(baseValue, toUnit);

        return new ConversionResult
        {
            InputValue   = value,
            FromUnit     = fromUnit.Id,
            FromUnitName = fromUnit.Name,
            OutputValue  = outputValue,
            ToUnit       = toUnit.Id,
            ToUnitName   = toUnit.Name,
            Category     = fromCategory.Name
        };
    }

    /// <inheritdoc />
    public IReadOnlyList<UnitCategory> GetCategories() => UnitRegistry.Categories;

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private static double ToBaseUnit(double value, Unit unit)
    {
        if (unit.ToBase is not null)
            return unit.ToBase(value);

        if (unit.Factor is null)
            throw new InvalidOperationException($"Unit '{unit.Id}' has neither a Factor nor a ToBase function.");

        return value * unit.Factor.Value;
    }

    private static double FromBaseUnit(double baseValue, Unit unit)
    {
        if (unit.FromBase is not null)
            return unit.FromBase(baseValue);

        if (unit.Factor is null)
            throw new InvalidOperationException($"Unit '{unit.Id}' has neither a Factor nor a FromBase function.");

        return baseValue / unit.Factor.Value;
    }
}
