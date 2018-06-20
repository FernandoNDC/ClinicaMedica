using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class RemedioPrescrito
    {
        [Key]
        public int RemedioPrescritoId { get; set; }

        [Display(Name = "Posologia")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Posologia { get; set; }

        [ForeignKey("Paciente")]
        public int? PacienteId { get; set; }
        public virtual Paciente Paciente { get; set; }

        [ForeignKey("Remedio")]
        public int? RemedioId { get; set; }
        public virtual Remedio Remedio { get; set; }

        [ForeignKey("Medico")]
        public int? MedicoId { get; set; }
        public virtual Medico Medico { get; set; }


    }
}