using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Model
{
    public partial class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public int InstituteId { get; set; }
        [JsonIgnore]
        public virtual Institute Institute { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
