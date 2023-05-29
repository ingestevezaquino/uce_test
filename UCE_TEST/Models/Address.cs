using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UCE_TEST.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        [Display(Name = "Calle")]
        [Required(ErrorMessage = "El campo 'Calle' es requerido.")]
        public string Street { get; set; } = null!;
        [Display(Name = "Sector")]
        [Required(ErrorMessage = "El campo 'Sector' es requerido.")]
        public string Zone { get; set; } = null!;
        [Display(Name = "Codigo Postal")]
        [Required(ErrorMessage = "El campo 'Codigo Postal' es requerido.")]
        public string? PostalCode { get; set; }
        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El campo 'Provincia' es requerido.")]
        public string Province { get; set; } = null!;
        [Display(Name = "Pais")]
        [Required(ErrorMessage = "El campo 'Pais' es requerido.")]
        public string Country { get; set; } = null!;
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
