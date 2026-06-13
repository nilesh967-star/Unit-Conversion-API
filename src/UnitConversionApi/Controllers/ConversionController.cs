using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

/// <summary>
/// Provides endpoints to convert values between units of measurement
/// and to discover the supported units.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConversionController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public ConversionController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    // POST /api/conversion
    /// <summary>Convert a value from one unit to another.</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/conversion
    ///     {
    ///         "value": 100,
    ///         "fromUnit": "kilometer",
    ///         "toUnit": "mile"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Conversion succeeded.</response>
    /// <response code="400">Invalid input or incompatible units.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConversionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ConversionResult> Convert([FromBody] ConversionRequest request)
    {
        try
        {
            var result = _conversionService.Convert(request.Value, request.FromUnit, request.ToUnit);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title  = "Invalid conversion request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    // GET /api/conversion/units
    /// <summary>List all supported unit categories and their units.</summary>
    /// <response code="200">Returns the list of categories.</response>
    [HttpGet("units")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<object>> GetUnits()
    {
        var categories = _conversionService.GetCategories()
            .Select(c => new
            {
                c.Id,
                c.Name,
                Units = c.Units.Select(u => new { u.Id, u.Name })
            });

        return Ok(categories);
    }

    // GET /api/conversion/units/{categoryId}
    /// <summary>List units within a specific category.</summary>
    /// <param name="categoryId">Category identifier (e.g. "length", "temperature").</param>
    /// <response code="200">Returns units in the category.</response>
    /// <response code="404">Category not found.</response>
    [HttpGet("units/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<object> GetUnitsByCategory(string categoryId)
    {
        var category = _conversionService.GetCategories()
            .FirstOrDefault(c => c.Id.Equals(categoryId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (category is null)
            return NotFound(new ProblemDetails
            {
                Title  = "Category not found",
                Detail = $"No category with ID '{categoryId}' exists.",
                Status = StatusCodes.Status404NotFound
            });

        return Ok(new
        {
            category.Id,
            category.Name,
            Units = category.Units.Select(u => new { u.Id, u.Name })
        });
    }
}
