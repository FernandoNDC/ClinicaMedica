using ClinicaMedica.Models;
using ClinicaMedica.Models.Entidades;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ClinicaMedica.Controllers.Controladores
{

    public class PacientesController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        // GET: Pacientes
        public ActionResult Index()
        {
            return View(db.Pacientes.OrderByDescending(p => p.Nome).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        // GET: Pacientes
        public ActionResult List()
        {
            return View(db.Pacientes.OrderByDescending(p => p.Nome).ToList());
        }

        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        // GET: Pacientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // GET: Pacientes/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PacienteId,Nome,Sobrenome,DataNascimento,CPF,Email,Telefone,Rua,Numero,Cidade")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Pacientes.Add(paciente);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_PACIENTE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PacienteId,Nome,Sobrenome,DataNascimento,CPF,Email,Telefone,Rua,Numero,Cidade")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Pacientes", new { id = paciente.PacienteId });
            }
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paciente paciente = db.Pacientes.Find(id);
            db.Pacientes.Remove(paciente);
            db.SaveChanges();
            return RedirectToAction("List");
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
