using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class UserAlarmRelation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AlarmsId { get; set; }

    //public DateTime CreatedAt { get; set; }

    //public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual Alarm? Alarms { get; set; } = null!;
    [JsonIgnore]

    public virtual User? IdNavigation { get; set; } = null!;
}
