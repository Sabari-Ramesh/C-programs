namespace Test.Modal
{
    public class UpdateEmployeeDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string email { get; set; }

        public string? phoneNumber { get; set; }
        public decimal salary { get; set; }
    }
}
