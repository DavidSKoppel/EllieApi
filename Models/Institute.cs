using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Institute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AddressId { get; set; }
    [JsonIgnore]

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]

    public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual Address? Address { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();
    [JsonIgnore]

    public virtual ICollection<Room>? Rooms { get; set; } = new List<Room>();
}
