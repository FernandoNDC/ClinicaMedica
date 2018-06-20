using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class Remedio
    {
        [Key]
        public int RemedioId { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Nome Genérico.")]
        [Display(Name = "Nome Genérico")]
        public string NomeGenerico { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Nome de Fábrica.")]
        [Display(Name = "Nome de Fábrica")]
        public string NomeFabrica { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Fabricante.")]
        [Display(Name = "Fabricante")]
        public string Fabricante { get; set; }
        public virtual List<RemedioPrescrito> RemedioPrescrito { get; set; }
    }
}