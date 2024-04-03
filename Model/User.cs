using System;
using System.Collections.Generic;

namespace EllieApi.Model;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Room { get; set; } = null!;

    public bool Active { get; set; }

    public int? Points { get; set; }

    public int ContactPersonId { get; set; }

    public virtual Employee ContactPerson { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual UserAlarmRelation? UserAlarmRelation { get; set; }
}
