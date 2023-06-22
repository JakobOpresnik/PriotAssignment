namespace InterviewAssignment.Services;

public class FastestLapService {
    private MeasurementService MeasurementService;

    public FastestLapService(MeasurementService measurementService) {
        MeasurementService = measurementService;
    }
}