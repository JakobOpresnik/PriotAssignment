using System.Diagnostics.Metrics;
using InterviewAssignement.Models;

namespace InterviewAssignment.Services;

public class FastestLapService {
    
    private MeasurementService _measurementService;

    public FastestLapService(MeasurementService measurementService) {
        _measurementService = measurementService;
    }
    
    // order laps by duration ascendingly
    public List<Lap> OrderLaps(List<Measurement> measurements)
    {
        // group measurements by RFIDs
        var grouped = measurements.GroupBy(obj => obj.rfid);

        List<Lap> laps = new List<Lap>();

        // loop through each group of measurements
        foreach (var group in grouped)
        {
            string groupRfid = group.Key;
            var list = group.ToList();

            // loop through all measurements in a group of measurements
            for (var i = 0; i < list.Count - 1; i++)
            {
                // 1 lap - 2 measurements (1 at start & 1 at finish)
                // lap duration - difference in time between 2 consecutive measurements with the same RFID
                TimeSpan duration = (list[i+1].date.TimeOfDay - list[i].date.TimeOfDay).Duration();
                Lap lap = new Lap
                {
                    rfid = groupRfid,
                    duration = duration,
                    measurements = list
                };
                laps.Add(lap);
            }
        }
        // sort laps by duration
        var sortedLaps = laps.OrderBy(lap => lap.duration);
        return sortedLaps.ToList();
    }
}