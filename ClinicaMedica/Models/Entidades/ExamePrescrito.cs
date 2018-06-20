using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class ExamePrescrito
    {
        [Key]
        public int ExamePrescritoId { get; set; }

        [Display(Name = "Detalhes")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Detalhes { get; set; }

        [Display(Name = "Resultado")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string ResultadoUrl { get; set; }

        [Display(Name = "ResultadoLiberado")]
        public bool ResultadoLiberado { get; set; }

        [ForeignKey("Paciente")]
        public int? PacienteId { get; set; }
        public virtual Paciente Paciente { get; set; }

        [ForeignKey("Exame")]
        public int? ExameId { get; set; }
        public virtual Exame Exame { get; set; }

        [ForeignKey("Medico")]
        public int? MedicoId { get; set; }
        public virtual Medico Medico { get; set; }

    }
}