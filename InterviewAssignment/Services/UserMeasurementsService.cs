using InterviewAssignement.Models;

namespace InterviewAssignment.Services;

public class UserMeasurementsService
{
    public List<Measurement> GetUserMeasurements(User user)
    {
        return user.measurements;
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
                rfid = userMeasurements[i].rfid,
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