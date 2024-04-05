using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Model
{
    public partial class Alarm
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime ActivatingTime { get; set; }
        public bool? AlarmType { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserAlarmRelation> UserAlarmRelations { get; set; } = new List<UserAlarmRelation>();
    }
}
