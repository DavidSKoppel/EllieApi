using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Address
{
    public int Id { get; set; }

    public int Postalcode { get; set; }

    public string Name { get; set; } = null!;

    public int Floor { get; set; }

    public int HouseNumber { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public DateTime? LastEdited { get; set; }

    [JsonIgnore]
    public virtual ICollection<Institute>? Institutes { get; set; } = new List<Institute>();
}
