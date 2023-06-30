using InterviewAssignement.Models;

namespace InterviewAssignment.Services;

public class UserMeasurementsService
{
    public List<Measurement> GetUserMeasurements(User user)
    {
        var allMeasurements = MeasurementService.measurements;
        var userMeasurements = new List<Measurement>();
        foreach (var measurement in allMeasurements)
        {
            if (measurement.rfid == user.id)
            {
                userMeasurements.Add(measurement);
            }
        }
        return userMeasurements;
    }

    public bool CheckUserMeasurement(User user, Measurement measurement)
    {
        if (user.id == measurement.rfid) return true;
        return false;
    }

    public List<Lap> OrderUserLaps(User user)
    {
        var userMeasurements = GetUserMeasurements(user);
        var userLaps = new List<Lap>();
        
        for (var i = 0; i < userMeasurements.Count - 1; i++)
        {
            TimeSpan duration = (userMeasurements[i + 1].date.TimeOfDay - userMeasurements[i].date.TimeOfDay).Duration();
            Lap lap = new Lap
            {
                rfid = user.id,
                duration = duration,
                measurements = userMeasurements
            };
            userLaps.Add(lap);
        }
        // sort user laps by duration
        var sortedUserLaps = userLaps.OrderBy(lap => lap.duration);
        return sortedUserLaps.ToList();
    }


}