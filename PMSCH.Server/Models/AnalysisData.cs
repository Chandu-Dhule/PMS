namespace PMSCH.Server.Models
{
    public class AnalysisData
    {
        public List<MachineHealthSummary> MachineHealthData { get; set; }
        public List<StatusCount> MaintenanceStatusData { get; set; }
        public List<CategoryCount> CategoryDistributionData { get; set; }
    }

    public class MachineHealthSummary
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public float Temp { get; set; }
    }

    public class StatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class CategoryCount
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }
}
