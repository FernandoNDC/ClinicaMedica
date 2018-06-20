using ClinicaMedica.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClinicaMedica.Models
{
    public class ClinicaDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Remedio> Remedios { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<RemedioPrescrito> RemedioPrescritos { get; set; }
        public DbSet<ExamePrescrito> ExamePrescritos { get; set; }
    }
}