using System.Diagnostics.Metrics;
using InterviewAssignement.Models;
using InterviewAssignment.Services;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace UnitTestProject;

public class FastestLapTests
{
    private FastestLapService _fastestLapService;
    private MeasurementService _measurementService;
    
    [SetUp]
    public void Setup()
    {
        // initialize services
        _measurementService = new MeasurementService();
        _fastestLapService = new FastestLapService(_measurementService);
    }

    [Test]
    public void SaveMeasurement()
    {
        // arrange
        var json = @"
            {
                'rfid': 'E00000001',
                'date': '2023-01-01T09:00:00.000Z'
            }";
        
        var measurement = JsonConvert.DeserializeObject<Measurement>(json);
        
        // act
        bool actual = _measurementService.HandlePayload(measurement);
        bool expected = true;
        
        // assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void SortLapsReturnSorted()
    {
        // arrange
        // test list of laps
        /*var laps = new List<Lap>
        {
            new Lap { duration = new TimeSpan(0, 10, 0) },
            new Lap { duration = new TimeSpan(0, 15, 0) },
            new Lap { duration = new TimeSpan(0, 5, 0) }
        };*/

        
        // JSON measurement test cases
        var jsons = new List<string>
        {
            @"{
                'rfid': 'E00000002',
                'date': '2023-01-01T09:00:00.000Z'
            }",
            @"{
                'rfid': 'E00000001',
                'date': '2023-01-01T09:01:00.000Z'
            }",
            @"{
                'rfid': 'E00000002',
                'date': '2023-01-01T09:05:00.000Z'
            }",
            @"
            {
                'rfid': 'E00000001',
                'date': '2023-01-01T09:00:00.000Z'
            }"
        };

        var measurements = new List<Measurement>();
        foreach (var json in jsons)
        {
            var measurement = JsonConvert.DeserializeObject<Measurement>(json);
            measurements.Add(measurement);
        }

        // act
        // use service to sort laps (actual output)
        var sortedLaps = _fastestLapService.OrderLaps(measurements);

        var expectedLaps = new List<TimeSpan>
        {
            new(0, 1, 0),
            new(0, 5, 0)
        };
        
        // select only durations from entire objects
        var actualLaps = sortedLaps.Select(lap => lap.duration).ToList();
        
        // assert
        // check for equality
        Assert.That(expectedLaps, Is.EqualTo(actualLaps));


        
        // expected output
        /*var expectedLaps = new List<TimeSpan> {
            new (0, 5, 0),
            new (0, 10, 0),
            new (0, 15, 0)
        };*/
        // filter out only duration
        //var actualLaps = sortedLaps.Select(lap => lap.duration).ToList();
    }

    [Test]
    public void SortEmptyReturnEmpty()
    {
        // arrange
        var measurements = new List<Measurement>();
        
        // act
        var sortedLaps = _fastestLapService.OrderLaps(measurements);
        
        // assert
        Assert.That(sortedLaps, Is.Empty);
    }
    
}