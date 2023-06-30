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
    private readonly UserMeasurementsService _userMeasurementsService;

    public MeasurementController(ILogger<MeasurementController> logger, MeasurementService measurementService, FastestLapService fastestLapService, UserMeasurementsService userMeasurementsService) {
        _logger = logger;
        _measurementService = measurementService;
        _fastestLapService = fastestLapService;
        _userMeasurementsService = userMeasurementsService;
    }

    
    // endpoint to get a measurement
    public IActionResult PostMeasurement([FromBody] string req)
    {
        // deserialize JSON string to C# object
        var measurement = JsonSerializer.Deserialize<Measurement>(req);
        
        if (_measurementService.HandlePayload(measurement))
        {
            // return status code 200 (success)
            return Ok();
        }
        // return status code 400 (error)
        return BadRequest(new { error = "Invalid RFID!" });
    }

    
    // endpoint to get sorted list of laps by duration
    public IActionResult GetFastestLaps([FromBody] string req)
    {
        var laps = JsonSerializer.Deserialize<List<Lap>>(req);

        var measurements = MeasurementService.measurements;
        
        var sortedLaps = _fastestLapService.OrderLaps(measurements);
        if (sortedLaps.Count == laps.Count)
        {
            return Ok();
        }
        return BadRequest(new { error = "Sorting failed!" });
    }
    
    
    // endpoint to get all measurements of a particular user
    public IActionResult GetUserMeasurements([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<User>(req);
        var userMeasurements = _userMeasurementsService.GetUserMeasurements(user);
        if (userMeasurements.Count == 0)
        {
            return BadRequest("This user has no measurements yet!");
        }
        return Ok(userMeasurements);
    }
    
    
    // endpoint to get all laps of a particular user sorted by duration
    public IActionResult GetSortedUserLaps([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<User>(req);
        var sortedUserLaps = _userMeasurementsService.OrderUserLaps(user);
        if (sortedUserLaps.Count == 0)
        {
            return BadRequest("This user has no laps yet!");
        }
        return Ok(sortedUserLaps);
    }
}