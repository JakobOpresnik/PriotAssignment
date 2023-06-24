namespace InterviewAssignement.Models;

public class Lap
{
    public string rfid { get; set; }
    public TimeSpan duration { get; set; }
    public List<Measurement> measurements { get; set; }
}