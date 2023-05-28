using System;
using System.Collections.Generic;

namespace UCE_TEST.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string Zone { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
