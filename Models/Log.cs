namespace EllieApi.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string TableName { get; set; } = null!;
        public DateTime? TimeEdited { get; set; }
    }
}
