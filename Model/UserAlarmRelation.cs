using System;
using System.Collections.Generic;

namespace EllieApi.Model;

public partial class UserAlarmRelation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AlarmsId { get; set; }

    public virtual Alarm Alarms { get; set; } = null!;

    public virtual User IdNavigation { get; set; } = null!;
}
