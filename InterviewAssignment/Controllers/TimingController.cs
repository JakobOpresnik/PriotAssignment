using InterviewAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class MeasurementController : ControllerBase {
    private readonly ILogger<MeasurementController> _logger;
    private readonly MeasurementService MeasurementService;
    private readonly FastestLapService _fastestLapService;

    public MeasurementController(ILogger<MeasurementController> logger, MeasurementService measurementService, FastestLapService fastestLapService) {
        _logger = logger;
        MeasurementService = measurementService;
        _fastestLapService = fastestLapService;
    }
}