using System.Text.Json;
using InterviewAssignement.Models;
using InterviewAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class MeasurementController : ControllerBase {
    private readonly ILogger<MeasurementController> _logger;
    private readonly MeasurementService _measurementService;
    private readonly FastestLapService _fastestLapService;

    public MeasurementController(ILogger<MeasurementController> logger, MeasurementService measurementService, FastestLapService fastestLapService) {
        _logger = logger;
        _measurementService = measurementService;
        _fastestLapService = fastestLapService;
    }


    private List<Measurement> memory = new List<Measurement>();
    
    // endpoint to get a measurement
    public IActionResult PostMeasurement([FromBody] string json)
    {
        // deserialize JSON string to C# object
        var measurement = JsonSerializer.Deserialize<Measurement>(json);
        
        if (_measurementService.HandlePayload(measurement))
        {
            // return status code 200 (success)
            return Ok();
        }
        // return status code 400 (error)
        return BadRequest(new { error = "Invalid RFID!" });
    }

    
    // endpoint to get sorted list of laps by duration
    public IActionResult GetFastestLaps([FromBody] string json)
    {
        var laps = JsonSerializer.Deserialize<List<Lap>>(json);

        var measurements = MeasurementService.measurements;
        
        var sortedLaps = _fastestLapService.OrderLaps(measurements);
        if (sortedLaps.Count == laps.Count)
        {
            return Ok();
        }
        return BadRequest(new { error = "Sorting failed!" });
    }
}