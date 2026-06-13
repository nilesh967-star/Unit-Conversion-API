namespace UnitConversionApi.Models;

/// <summary>
/// A group of related units that can be converted among each other (e.g. length, weight).
/// </summary>
public class UnitCategory
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<Unit> Units { get; init; } = [];
}
