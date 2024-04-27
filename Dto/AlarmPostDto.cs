using EllieApi.Models;
using System.Text.Json.Serialization;

namespace EllieApi.Dto
{
    public class AlarmPostDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime ActivatingTime { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public int AlarmTypeId { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? LastEdited { get; set; }

        public bool Active { get; set; }

        public int[]? UserIds { get; set; } = null;

        public bool IsAllUsersChecked { get; set; } = false;
    }
}
