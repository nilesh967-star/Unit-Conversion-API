using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public interface IConversionService
{
    /// <summary>
    /// Converts <paramref name="value"/> from <paramref name="fromUnitId"/> to <paramref name="toUnitId"/>.
    /// </summary>
    /// <returns>A <see cref="ConversionResult"/> on success.</returns>
    /// <exception cref="ArgumentException">Thrown when a unit ID is unknown or the units belong to different categories.</exception>
    ConversionResult Convert(double value, string fromUnitId, string toUnitId);

    /// <summary>Returns all supported categories and their units.</summary>
    IReadOnlyList<UnitCategory> GetCategories();
}
