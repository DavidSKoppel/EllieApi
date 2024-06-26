﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Model
{
    public partial class Address
    {
        public int Id { get; set; }

        public int Postalcode { get; set; }

        public string Name { get; set; } = null!;

        public int Floor { get; set; }

        public int HouseNumber { get; set; }
        [JsonIgnore]
        public virtual ICollection<Institute> Institutes { get; set; } = new List<Institute>();
    }
}
