using System;
using System.Collections.Generic;

namespace UCE_TEST.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime BirthDay { get; set; }
        public DateTime DateOfHire { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CivilStatus { get; set; } = null!;
        public byte[]? Photo { get; set; }

        public virtual Address? Address { get; set; }
    }
}
