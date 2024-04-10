using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class AlarmType
{
    public int Id { get; set; }

    public string? Name { get; set; }
    [JsonIgnore]

    public virtual ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();
}
