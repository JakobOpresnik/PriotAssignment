﻿namespace InterviewAssignement.Models;

public class User
{
    public string id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string passwordHash { get; set; }
    public string token { get; set; }
    public List<Measurement> measurements { get; set; }
}