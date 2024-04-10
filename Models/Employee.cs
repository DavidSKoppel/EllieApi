using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public int RoleId { get; set; }

    public int InstituteId { get; set; }

    //public DateTime CreatedAt { get; set; }

    //public DateTime? LastEdited { get; set; }
    [JsonIgnore]

    public virtual Institute? Institute { get; set; } = null!;
    [JsonIgnore]

    public virtual Role? Role { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
