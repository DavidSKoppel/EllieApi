using System;
using System.Collections.Generic;

namespace EllieApi.Model;

public partial class Alarm
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime ActivatingTime { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserAlarmRelation> UserAlarmRelations { get; set; } = new List<UserAlarmRelation>();
}
