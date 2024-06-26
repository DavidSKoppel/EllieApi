﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EllieApi.Model
{
    public partial class Institute
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int AddressId { get; set; }
        [JsonIgnore]
        public virtual Address Address { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
