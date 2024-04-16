using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? UserId { get; set; }

    public int InstituteId { get; set; }
    [JsonIgnore]

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]

    public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual Institute? Institute { get; set; } = null!;
    [JsonIgnore]

    public virtual User? User { get; set; }
}
