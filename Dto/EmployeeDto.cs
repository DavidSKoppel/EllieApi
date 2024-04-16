namespace EllieApi.Dto
{
    public class EmployeeDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        public bool Active { get; set; }

        public int InstituteId { get; set; }

        public int RoleId { get; set; }
    }
}