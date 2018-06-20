using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaMedica.Models;
using ClinicaMedica.Models.Entidades;

namespace ClinicaMedica.Controllers.Controladores
{
    [Authorize]
    public class ExamePrescritosController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        // GET: ExamePrescritos
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Index()
        {
            var examePrescritos = db.ExamePrescritos.Include(e => e.Exame).Include(e => e.Medico).Include(e => e.Paciente);
            return View(examePrescritos.ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult List()
        {
            var examePrescritos = db.ExamePrescritos.Include(e => e.Exame).Include(e => e.Medico).Include(e => e.Paciente);
            return View(examePrescritos.ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarExamePrescrito(int? PacienteId)
        {
            return PartialView(db.ExamePrescritos.Where(p => p.PacienteId == PacienteId).OrderByDescending(p => p.Exame.Nome).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult ListarExamePrescritoP(int? PacienteId)
        {
            return PartialView(db.ExamePrescritos.Where(p => p.PacienteId == PacienteId).OrderByDescending(p => p.Exame.Nome).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult ListarExamePrescritoM(int? MedicoId)
        {
            return PartialView(db.ExamePrescritos.Where(p => p.MedicoId == MedicoId).OrderByDescending(p => p.Exame.Nome).ToList());
        }

        // GET: RemedioPrescritos/Requisitar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Requisitar(int? MedicoId)
        {
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome");
            ViewBag.MedicoId = MedicoId;
            ViewBag.ExameLista = db.Exames.OrderBy(m => m.Nome).ToList();
            return PartialView();
        }


        // GET: ExamePrescritos/Details/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            if (examePrescrito == null)
            {
                return HttpNotFound();
            }
            return View(examePrescrito);
        }

        // POST: RemedioPrescritos/Adicionar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        public ActionResult Adicionar(int? exameId, int? medicoId, int? pacienteID, string detalhes)
        {
            if (exameId == null || medicoId == null || pacienteID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Exame exame = db.Exames.Find(exameId);
            Medico medico = db.Medicos.Find(medicoId);
            Paciente paciente = db.Pacientes.Find(pacienteID);
            if (exame == null || medico == null || paciente == null)
            {
                return HttpNotFound();
            }
            ExamePrescrito examePrescrito = new ExamePrescrito();
            examePrescrito.MedicoId = medico.MedicoId;
            examePrescrito.ExameId = exame.ExameID;
            examePrescrito.PacienteId = paciente.PacienteId;
            examePrescrito.Detalhes = detalhes;

            db.ExamePrescritos.Add(examePrescrito);
            db.SaveChanges();

            //return View();
            return Json(examePrescrito.ExamePrescritoId, JsonRequestBehavior.AllowGet);
        }

        // POST: RemedioPrescritos/Liberar
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        public ActionResult Liberar(int? examePrescritoId, int? exameId, int? medicoId, int? pacienteId, bool resultadoLiberado)
        {
            if (exameId == null || medicoId == null || pacienteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.ProxyCreationEnabled = false;
            Exame exame = db.Exames.Find(exameId);
            Medico medico = db.Medicos.Find(medicoId);
            Paciente paciente = db.Pacientes.Find(pacienteId);
            if (exame == null || medico == null || paciente == null)
            {
                return HttpNotFound();
            }
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(examePrescritoId);
            examePrescrito.MedicoId = medico.MedicoId;
            examePrescrito.ExameId = exame.ExameID;
            examePrescrito.PacienteId = paciente.PacienteId;
            examePrescrito.ResultadoLiberado = resultadoLiberado;

            db.Entry(examePrescrito).State = EntityState.Modified;
            db.SaveChanges();

            //return View();
            return Json(examePrescrito.ExamePrescritoId, JsonRequestBehavior.AllowGet);
        }


        // GET: ExamePrescritos/Upload/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Upload(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            if (examePrescrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.examePrescrito = examePrescrito;
            return View(examePrescrito);
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "ExamePrescritoId,Detalhes,ResultadoUrl,ResultadoLiberado,PacienteId,ExameId,MedicoId")] ExamePrescrito examePrescrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examePrescrito).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("List");
            }
            return Redirect("List");
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        // GET: ExamePrescritos/Create
        public ActionResult Create()
        {
            ViewBag.ExameId = new SelectList(db.Exames, "ExameID", "Nome");
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome");
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome");
            return View();
        }

        // POST: ExamePrescritos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamePrescritoId,Detalhes,File,ResultadoLiberado,PacienteId,ExameId,MedicoId")] ExamePrescrito examePrescrito)
        {
            if (ModelState.IsValid)
            {
                db.ExamePrescritos.Add(examePrescrito);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.ExameId = new SelectList(db.Exames, "ExameID", "Nome", examePrescrito.ExameId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", examePrescrito.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", examePrescrito.PacienteId);
            return View(examePrescrito);
        }

        // GET: ExamePrescritos/Edit/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            if (examePrescrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.examePrescrito = examePrescrito;
            return View(examePrescrito);
        }

        // POST: ExamePrescritos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamePrescritoId,Detalhes,File,ResultadoLiberado,PacienteId,ExameId,MedicoId")] ExamePrescrito examePrescrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examePrescrito).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.ExameId = new SelectList(db.Exames, "ExameID", "Nome", examePrescrito.ExameId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "MedicoId", "Nome", examePrescrito.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "PacienteId", "Nome", examePrescrito.PacienteId);
            return View(examePrescrito);
        }

        // GET: ExamePrescritos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            if (examePrescrito == null)
            {
                return HttpNotFound();
            }
            return View(examePrescrito);
        }

        // POST: ExamePrescritos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            db.ExamePrescritos.Remove(examePrescrito);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        public ActionResult DeleteConsultando(int id)
        {
            ExamePrescrito examePrescrito = db.ExamePrescritos.Find(id);
            db.ExamePrescritos.Remove(examePrescrito);
            db.SaveChanges();
            return Json(examePrescrito.ExamePrescritoId, JsonRequestBehavior.AllowGet);
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
