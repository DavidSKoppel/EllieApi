using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Alarm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime ActivatingTime { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Description { get; set; }
    public int AlarmTypeId { get; set; }
    public bool Active { get; set; }
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    public DateTime? LastEdited { get; set; }
    [JsonIgnore]
    public virtual AlarmType? AlarmType { get; set; } = null!;
    public virtual ICollection<UserAlarmRelation>? UserAlarmRelations { get; set; } = new List<UserAlarmRelation>();
}
