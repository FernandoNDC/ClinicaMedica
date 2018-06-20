using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaMedica.Models;
using ClinicaMedica.Models.Entidades;

namespace ClinicaMedica.Controllers.Controladores
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        // GET: Consultas
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Index()
        {
            var consultas = db.Consultas.Include(c => c.Medico).Include(c => c.Paciente);
            return View(consultas.ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult List()
        {
            var consultas = db.Consultas.Include(c => c.Medico).Include(c => c.Paciente);
            return View(consultas.ToList());
        }

        // GET: Consultas/Details/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        // GET: Consultas/Create
        public ActionResult Create()
        {
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome");
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome");
            return View();
        }

        // POST: Consultas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConsultaId,DataConsulta,PacienteId,MedicoId,Anamnese,Status")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                db.Consultas.Add(consulta);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // GET: Consultas/Agendar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN+", "+ Constants.Constants.PROFILE_PACIENTE)]
        [AllowAnonymous]
        public ActionResult Agendar(int? PacienteId)
        {
            ViewBag.MedicoLista = db.Medicos.OrderBy(m => m.Nome).ToList();
            ViewBag.PacienteId = PacienteId;
            return PartialView();
        }

        // POST: Consultas/Agendar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Agendar(DateTime dataConsulta, int? medicoId, int? pacienteId)
        {
            if (medicoId == null || pacienteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Medico medico = db.Medicos.Find(medicoId);
            Paciente paciente = db.Pacientes.Find(pacienteId);
            if (medico == null || paciente == null)
            {
                return HttpNotFound();
            }
            Consulta consulta = new Consulta();
            consulta.MedicoId = medico.MedicoId;
            consulta.PacienteId = paciente.PacienteId;
            consulta.Status = 1;
            consulta.DataConsulta = dataConsulta;

            db.Consultas.Add(consulta);
            db.SaveChanges();

            //return View();
            return Json(consulta.ConsultaId, JsonRequestBehavior.AllowGet);

        }

        // GET: Consultas/Confirmar/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [AllowAnonymous]
        public ActionResult Confirmar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // POST: Consultas/Confirmar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmar(int? consultaId, int?teste)
        {
            if (consultaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(consultaId);
            if (consulta == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                consulta.Status = 2;
                db.Entry(consulta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Medicos", new { id = consulta.MedicoId}); ;
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // GET: Consultas/Consultar/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [AllowAnonymous]
        public ActionResult Consultar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // POST: Consultas/Consultar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Consultar(int? consultaId, string Anamnese)
        {
            if (consultaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(consultaId);
            if (consulta == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                consulta.Anamnese = Anamnese;
                consulta.Status = 3;
                db.Entry(consulta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Medicos", new { id = consulta.MedicoId });
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult ListarAgendadasP(int? PacienteId)
        {
            return PartialView(db.Consultas
                .Where(p => p.PacienteId == PacienteId)
                .Where(p => p.Status != 3)
                .OrderByDescending(p => p.DataConsulta).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarAgendadasM(int? MedicoId)
        {
            return PartialView(db.Consultas
                .Where(p => p.MedicoId == MedicoId)
                .Where(p => p.Status != 3)
                .OrderByDescending(p => p.DataConsulta).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult ListarRealizadasP(int? PacienteId)
        {
            return PartialView(db.Consultas
                            .Where(p => p.PacienteId == PacienteId)
                            .Where(p => p.Status == 3)
                            .OrderByDescending(p => p.DataConsulta).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarRealizadasM(int? MedicoId)
        {
            return PartialView(db.Consultas
                .Where(p => p.MedicoId == MedicoId)
                .Where(p => p.Status == 3)
                .OrderByDescending(p => p.DataConsulta).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult DatasMedicoPaciente(int? medicoId, int? pacienteId, string data)
        {
            var inicioD = DateTime.ParseExact(data, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var fimD = inicioD.AddDays(1);

            if (medicoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (pacienteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var consulta = db.Consultas.Where(c => c.MedicoId == medicoId
                                                       || c.PacienteId == pacienteId
                                                       && c.DataConsulta > inicioD
                                                       && c.DataConsulta < fimD);

            var horarios = ConfigurationManager.AppSettings["Horarios"].Split(',');
            var dataIndisponivel = new string[] { }.ToList();
            foreach (Consulta c in consulta)
            {
                dataIndisponivel.Add(c.DataConsulta.Hour.ToString().PadLeft(2, '0')
                                  + ":"
                                  + c.DataConsulta.Minute.ToString().PadLeft(2, '0')
                                  );
            }
            var dataDisponivel = horarios.Except(dataIndisponivel);

            return Json(dataDisponivel, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConsultaId,DataConsulta,PacienteId,MedicoId,Anamnese,Status")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consulta).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", consulta.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // GET: Consultas/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        // POST: Consultas/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consulta consulta = db.Consultas.Find(id);
            db.Consultas.Remove(consulta);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
