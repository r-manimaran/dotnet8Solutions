namespace cartesionExplosion_api.Entities
{
    public class SalaryPayment
    {
        public int Id { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }

        public int EmployeeId { get; set; }
    }
}
