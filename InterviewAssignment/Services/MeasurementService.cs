using InterviewAssignement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Services;

public class MeasurementService
{
    public static List<Measurement> measurements = new List<Measurement>();

    public bool HandlePayload(Measurement measurement)
    {
        // check for valid RFID
        if (measurement.rfid[0] == 'E' && measurement.rfid.Length == 9)
        {
            var allUsers = RegisterService.allUsers;
            foreach (var user in allUsers)
            {
                // add measurement to logged in user
                if (user.token == "")
                {
                    user.measurements.Add(measurement);
                }
            }
            
            // save incoming measurement
            measurements.Add(measurement);
            return true;
        }
        return false;
    }
}