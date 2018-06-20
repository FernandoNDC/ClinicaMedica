using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models.Entidades
{
    public class Medico
    {
        [Key]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Nome.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Sobrenome.")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Especialização.")]
        [Display(Name = "Especialização")]
        public string Especializacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a Data de Nascimento.")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DataNascimento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o CPF.")]
        [Display(Name = "CPF")]
        [StringLength(11, ErrorMessage = "CPF inválido.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Email.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Telefone.")]
        [Display(Name = "Telefone")]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Display(Name = "Rua")]
        public string Rua { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }

        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        public virtual List<Consulta> Consulta { get; set; }

        public virtual List<RemedioPrescrito> RemedioPrescrito { get; set; }

    }
}