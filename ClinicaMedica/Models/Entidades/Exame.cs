using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class Exame
    {
        [Key]
        public int ExameID { get; set; }

        [Display(Name = "Nome")]
        [Required]
        [StringLength(30)]
        public string Nome { get; set; }

        public virtual List<ExamePrescrito> ExamePrescrito { get; set; }
    }
}