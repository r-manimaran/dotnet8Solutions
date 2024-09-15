namespace cartesionExplosion_api.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Budget { get; set; }
        public DateTime EstablishedDate { get; set; }
        public string HeadOfDepartment { get; set; }
        public bool IsActive { get; set; }
        public List<Team> Teams { get; set; } = new();
    }
}
