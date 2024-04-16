using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]

    public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();
}
