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
    public class MedicosController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        // GET: Medicos
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Index()
        {
            return View(db.Medicos.ToList());
        }

        // GET: Medicos
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult List()
        {
            return View(db.Medicos.ToList());
        }

        // GET: Medicos/Details/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }


        // GET: Medicos/Create
        [AllowAnonymous]
        public ActionResult CreateR()
        {
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateR(string Nome, string Sobrenome, string Especializacao,
            DateTime DataNascimento, string CPF, string Email, string Telefone, string Rua,
            int Numero, string Cidade)
        {

            db.Configuration.ProxyCreationEnabled = false;
            Medico medico = new Medico();
            medico.Nome = Nome;
            medico.Sobrenome = Sobrenome;
            medico.Especializacao = Especializacao;
            medico.DataNascimento = DataNascimento;
            medico.CPF = CPF;
            medico.Email = Email;
            medico.Telefone = Telefone;
            medico.Rua = Rua;
            medico.Numero = Numero;
            medico.Cidade = Cidade;

            db.Medicos.Add(medico);
            db.SaveChanges();

            return RedirectToAction("Details", "Medicos", medico.MedicoId);
        }



        // GET: Medicos/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicoId,Nome,Sobrenome,Especializacao,DataNascimento,CPF,Email,Telefone,Rua,Numero,Cidade")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Medicos.Add(medico);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(medico);
        }

        // GET: Medicos/Edit/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN + ", " + Constants.Constants.PROFILE_MEDICO)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicoId,Nome,Sobrenome,Especializacao,DataNascimento,CPF,Email,Telefone,Rua,Numero,Cidade")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Medicos", new { id = medico.MedicoId });
            }
            return View(medico);
        }

        // GET: Medicos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Delete/5
        [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medico medico = db.Medicos.Find(id);
            db.Medicos.Remove(medico);
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
