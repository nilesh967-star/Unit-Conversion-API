using FluentAssertions;
using UnitConversionApi.Services;
using Xunit;

namespace UnitConversionApi.Tests.Services;

public class ConversionServiceTests
{
    private readonly ConversionService _sut = new();

    // ---- Length ----

    [Fact]
    public void Convert_MetersToFeet_ReturnsCorrectValue()
    {
        var result = _sut.Convert(1, "meter", "foot");

        result.OutputValue.Should().BeApproximately(3.28084, precision: 0.0001);
    }

    [Fact]
    public void Convert_KilometersToMiles_ReturnsCorrectValue()
    {
        var result = _sut.Convert(1, "kilometer", "mile");

        result.OutputValue.Should().BeApproximately(0.621371, precision: 0.0001);
    }

    // ---- Weight ----

    [Fact]
    public void Convert_KilogramsToPounds_ReturnsCorrectValue()
    {
        var result = _sut.Convert(1, "kilogram", "pound");

        result.OutputValue.Should().BeApproximately(2.20462, precision: 0.0001);
    }

    // ---- Temperature ----

    [Fact]
    public void Convert_CelsiusToFahrenheit_BoilingPoint()
    {
        var result = _sut.Convert(100, "celsius", "fahrenheit");

        result.OutputValue.Should().BeApproximately(212, precision: 0.0001);
    }

    [Fact]
    public void Convert_CelsiusToFahrenheit_FreezingPoint()
    {
        var result = _sut.Convert(0, "celsius", "fahrenheit");

        result.OutputValue.Should().BeApproximately(32, precision: 0.0001);
    }

    [Fact]
    public void Convert_FahrenheitToCelsius_BodyTemperature()
    {
        var result = _sut.Convert(98.6, "fahrenheit", "celsius");

        result.OutputValue.Should().BeApproximately(37, precision: 0.0001);
    }

    [Fact]
    public void Convert_CelsiusToKelvin()
    {
        var result = _sut.Convert(0, "celsius", "kelvin");

        result.OutputValue.Should().BeApproximately(273.15, precision: 0.0001);
    }

    // ---- Volume ----

    [Fact]
    public void Convert_LitersToGallonsUs()
    {
        var result = _sut.Convert(1, "liter", "gallon_us");

        result.OutputValue.Should().BeApproximately(0.264172, precision: 0.0001);
    }

    // ---- Same unit (identity) ----

    [Fact]
    public void Convert_SameUnit_ReturnsSameValue()
    {
        var result = _sut.Convert(42, "meter", "meter");

        result.OutputValue.Should().Be(42);
    }

    // ---- Error cases ----

    [Fact]
    public void Convert_UnknownFromUnit_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(1, "unknown_unit", "meter");

        act.Should().Throw<ArgumentException>().WithMessage("*unknown_unit*");
    }

    [Fact]
    public void Convert_UnknownToUnit_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(1, "meter", "unknown_unit");

        act.Should().Throw<ArgumentException>().WithMessage("*unknown_unit*");
    }

    [Fact]
    public void Convert_IncompatibleCategories_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(1, "meter", "kilogram");

        act.Should().Throw<ArgumentException>();
    }
}
