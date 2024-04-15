using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class UserAlarmRelation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AlarmsId { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public DateTime? LastEdited { get; set; }

    [JsonIgnore]
    public virtual Alarm? Alarms { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
