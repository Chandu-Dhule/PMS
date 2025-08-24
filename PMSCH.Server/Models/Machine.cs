namespace PMSCH.Server.Models
{
    public class Machine
    {
        public int MachineID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public DateTime InstallationDate { get; set; }
        public string Status { get; set; }

        public int LifeCycle { get; set; }
        public float? Temperature { get; set; }
        public float? EnergyConsumption { get; set; }
        public string? HealthStatus { get; set; }

        public string? AssignedTo { get; set; }


    }

}
