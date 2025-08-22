public class MaintenanceLog
{
    public int LogID { get; set; }
    public int MachineID { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string Description { get; set; }
    public string OperatorName { get; set; }
    public DateTime NextDueDate { get; set; }

    // New fields from form
    public float Temperature { get; set; }
    public float EnergyConsumption { get; set; }
    public string HealthStatus { get; set; }
}
