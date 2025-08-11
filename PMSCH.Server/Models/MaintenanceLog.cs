namespace PMSCH.Server.Models
{
    public class MaintenanceLog
    {
        public int LogID { get; set; }
        public int MachineID { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string Description { get; set; }
        public string OperatorName { get; set; }
        public DateTime NextDueDate { get; set; }
    }

}
