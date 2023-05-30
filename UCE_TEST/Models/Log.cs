using System;
using System.Collections.Generic;

namespace UCE_TEST.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int EmployeeId { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
