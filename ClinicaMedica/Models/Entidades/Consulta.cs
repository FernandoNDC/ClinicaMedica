using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class Consulta
    {
        [Key]
        public int ConsultaId { get; set; }

        [Required(ErrorMessage = "Obrigatório selcionar a data da consulta.")]
        [Display(Name = "Data da consulta")]
        [DataType(DataType.DateTime)]
        public DateTime DataConsulta { get; set; }

        [Required]
        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }
        public virtual Paciente Paciente { get; set; }

        [Required]
        [ForeignKey("Medico")]
        public int MedicoId { get; set; }
        public virtual Medico Medico { get; set; }

        [Display(Name = "Anamnese")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Anamnese { get; set; }

        public int Status { get; set; }

    }
}