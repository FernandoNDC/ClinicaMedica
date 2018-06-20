using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaMedica.Models;
using ClinicaMedica.Models.Entidades;

namespace ClinicaMedica.Controllers.Controladores
{
    [Authorize]
    public class RemedioPrescritosController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        // GET: RemedioPrescritos
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Index()
        {
            var remedioPrescritos = db.RemedioPrescritos.Include(r => r.Medico).Include(r => r.Paciente).Include(r => r.Remedio);
            return View(remedioPrescritos.ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        // GET: RemedioPrescritos
        public ActionResult List()
        {
            return View(db.RemedioPrescritos.ToList());
        }

        // GET: RemedioPrescritos/Receitar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN+", "+ Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Receitar(int? MedicoId)
        {
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome");
            ViewBag.MedicoId = MedicoId;
            ViewBag.RemedioLista = db.Remedios.OrderBy(m => m.NomeFabrica).ToList();
            return PartialView();
        }

        // POST: RemedioPrescritos/Adicionar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        public ActionResult Adicionar(int? remedioId, int? medicoId, int? pacienteID, string posologia)
        {
            if (remedioId == null || medicoId == null || pacienteID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Remedio remedio = db.Remedios.Find(remedioId);
            Medico medico = db.Medicos.Find(medicoId);
            Paciente paciente = db.Pacientes.Find(pacienteID);
            if (remedio == null || medico == null || paciente == null)
            {
                return HttpNotFound();
            }
            RemedioPrescrito remedioPrescrito = new RemedioPrescrito();
            remedioPrescrito.MedicoId = medico.MedicoId;
            remedioPrescrito.RemedioId = remedio.RemedioId;
            remedioPrescrito.PacienteId = paciente.PacienteId;
            remedioPrescrito.Posologia = posologia;

            db.RemedioPrescritos.Add(remedioPrescrito);
            db.SaveChanges();

            //return View();
            return Json(remedioPrescrito.RemedioPrescritoId, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarRemedioPrescrito(int? PacienteId)
        {
            return PartialView(db.RemedioPrescritos.Where(p => p.PacienteId == PacienteId).OrderByDescending(p => p.Remedio.NomeFabrica).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult ListarRemedioPrescritoP(int? PacienteId)
        {
            return PartialView(db.RemedioPrescritos.Where(p => p.PacienteId == PacienteId).OrderByDescending(p => p.Remedio.NomeFabrica).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarRemedioPrescritoM(int? MedicoId)
        {
            return PartialView(db.RemedioPrescritos.Where(p => p.MedicoId == MedicoId).OrderByDescending(p => p.Remedio.NomeFabrica).ToList());
        }

        // GET: RemedioPrescritos/Details/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemedioPrescrito remedioPrescrito = db.RemedioPrescritos.Find(id);
            if (remedioPrescrito == null)
            {
                return HttpNotFound();
            }
            return View(remedioPrescrito);
        }

        // GET: RemedioPrescritos/Create
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Create()
        {
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome");
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome");
            ViewBag.RemedioId = new SelectList(db.Remedios, "RemedioId", "NomeGenerico");
            return View();
        }

        // POST: RemedioPrescritos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RemedioPrescritoId,Posologia,PacienteId,RemedioId,MedicoId")] RemedioPrescrito remedioPrescrito)
        {
            if (ModelState.IsValid)
            {
                db.RemedioPrescritos.Add(remedioPrescrito);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", remedioPrescrito.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", remedioPrescrito.PacienteId);
            ViewBag.RemedioId = new SelectList(db.Remedios, "RemedioId", "NomeGenerico", remedioPrescrito.RemedioId);
            return View(remedioPrescrito);
        }


        // GET: RemedioPrescritos/Edit/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemedioPrescrito remedioPrescrito = db.RemedioPrescritos.Find(id);
            if (remedioPrescrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.remedioPrescrito = remedioPrescrito;
            return View(remedioPrescrito);
        }

        // POST: RemedioPrescritos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RemedioPrescritoId,Posologia,PacienteId,RemedioId,MedicoId")] RemedioPrescrito remedioPrescrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remedioPrescrito).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", remedioPrescrito.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", remedioPrescrito.PacienteId);
            ViewBag.RemedioId = new SelectList(db.Remedios, "RemedioId", "NomeGenerico", remedioPrescrito.RemedioId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: RemedioPrescritos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RemedioPrescrito remedioPrescrito = db.RemedioPrescritos.Find(id);
            if (remedioPrescrito == null)
            {
                return HttpNotFound();
            }
            return View(remedioPrescrito);
        }

        // POST: RemedioPrescritos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RemedioPrescrito remedioPrescrito = db.RemedioPrescritos.Find(id);
            db.RemedioPrescritos.Remove(remedioPrescrito);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        // POST: RemedioPrescritos/DeleteConsultando/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        public ActionResult DeleteConsultando(int id)
        {
            RemedioPrescrito remedioPrescrito = db.RemedioPrescritos.Find(id);
            db.RemedioPrescritos.Remove(remedioPrescrito);
            db.SaveChanges();
            return Json(remedioPrescrito.RemedioPrescritoId, JsonRequestBehavior.AllowGet);
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
