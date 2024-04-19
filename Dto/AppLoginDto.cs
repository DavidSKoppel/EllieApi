namespace EllieApi.Dto
{
    public class AppLoginDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Points { get; set; }
        public string Token { get; set; }
    }
}
