using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Institute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AddressId { get; set; }

    //public DateTime CreatedAt { get; set; }

    //public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual Address? Address { get; set; }
    [JsonIgnore]

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
