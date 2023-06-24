using System.Diagnostics.Metrics;
using InterviewAssignement.Models;

namespace InterviewAssignment.Services;

public class FastestLapService {
    private MeasurementService MeasurementService;

    public FastestLapService(MeasurementService measurementService) {
        MeasurementService = measurementService;
    }
    
    // order laps by duration ascendingly
    public List<Lap> OrderLaps(List<Measurement> measurements)
    {
        var grouped = measurements.GroupBy(obj => obj.rfid);

        List<Lap> laps = new List<Lap>();

        foreach (var group in grouped)
        {
            string group_rfid = group.Key;
            var list = group.ToList();

            if (list.Count == 2)
            {
                TimeSpan duration = (list[1].date.TimeOfDay - list[0].date.TimeOfDay).Duration();
                Lap lap = new Lap
                {
                    rfid = group_rfid,
                    duration = duration,
                    measurements = list
                };
                laps.Add(lap);
            }
        }
        var sortedLaps = laps.OrderBy(lap => lap.duration);
        return sortedLaps.ToList();
    }
}