using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Model
{
    public partial class Note
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}
