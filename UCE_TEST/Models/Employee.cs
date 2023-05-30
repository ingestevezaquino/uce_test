using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UCE_TEST.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo 'Nombre' es requerido.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo 'Apellido' es requerido.")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Posición")]
        [Required(ErrorMessage = "El campo 'Posición' es requerido.")]
        public string Position { get; set; } = null!;
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "El campo 'Fecha de Nacimiento' es requerido.")]
        public DateTime BirthDay { get; set; }
        [Display(Name = "Fecha de Contratación")]
        [Required(ErrorMessage = "El campo 'Fecha de Contratación' es requerido.")]
        public DateTime DateOfHire { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo 'Teléfono' es requerido.")]
        [MaxLength(10, ErrorMessage = "El campo 'Teléfono' admite un máximo de 10 numeros.")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "El campo 'Teléfono' solo admite numeros del 0-9.")]
        public string Phone { get; set; } = null!;
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo 'Correo' es requerido.")]
        [EmailAddress(ErrorMessage = "El campo 'Correo' no contiene una dirección de e-mail válida.")]
        public string Email { get; set; } = null!;
        [Display(Name = "Estado Civil")]
        [Required(ErrorMessage = "El campo 'Estado Civil' es requerido.")]
        public string MaritalStatus { get; set; } = null!;
        [Display(Name = "Estado")]
        public bool? IsActive { get; set; }
        [Display(Name = "Foto")]
        public byte[]? Photo { get; set; }

        public virtual Address? Address { get; set; }
    }
}
