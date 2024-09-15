namespace cartesionExplosion_api.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MissionStatement { get; set; }
        public int TargetProductivity { get; set; }
        public DateTime FormationDate { get; set; }
        public string TeamLead { get; set; }
        public bool IsCrossFunctional { get; set; }

        public int DepartmentId { get; set; }

        public List<Employee> Employees { get; set; } = new();
    }
}
