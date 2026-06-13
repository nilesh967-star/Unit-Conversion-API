using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversionApi.Models;
using Xunit;

namespace UnitConversionApi.Tests.Controllers;

public class ConversionControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConversionControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostConversion_ValidRequest_Returns200WithResult()
    {
        var request = new ConversionRequest
        {
            Value    = 100,
            FromUnit = "kilometer",
            ToUnit   = "mile"
        };

        var response = await _client.PostAsJsonAsync("/api/conversion", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ConversionResult>();
        result.Should().NotBeNull();
        result!.OutputValue.Should().BeApproximately(62.1371, precision: 0.001);
        result.Category.Should().Be("Length");
    }

    [Fact]
    public async Task PostConversion_UnknownUnit_Returns400()
    {
        var request = new ConversionRequest
        {
            Value    = 1,
            FromUnit = "nonsense",
            ToUnit   = "meter"
        };

        var response = await _client.PostAsJsonAsync("/api/conversion", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostConversion_IncompatibleUnits_Returns400()
    {
        var request = new ConversionRequest
        {
            Value    = 1,
            FromUnit = "meter",
            ToUnit   = "kilogram"
        };

        var response = await _client.PostAsJsonAsync("/api/conversion", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUnits_Returns200WithCategories()
    {
        var response = await _client.GetAsync("/api/conversion/units");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUnitsByCategory_ValidCategory_Returns200()
    {
        var response = await _client.GetAsync("/api/conversion/units/length");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUnitsByCategory_InvalidCategory_Returns404()
    {
        var response = await _client.GetAsync("/api/conversion/units/invalidcategory");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
