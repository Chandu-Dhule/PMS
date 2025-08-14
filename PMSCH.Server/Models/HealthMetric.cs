namespace PMSCH.Server.Models
{
    public class HealthMetric
    {
        public int MetricID { get; set; }
        public int MachineID { get; set; }
        public DateTime CheckDate { get; set; }
        public float Temperature { get; set; }
        public float EnergyConsumption { get; set; }
        public string HealthStatus { get; set; }
    }

}
