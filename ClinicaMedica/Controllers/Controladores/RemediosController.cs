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
    [Authorize(Roles = Constants.Constants.PROFILE_ADMIN)]
    public class RemediosController : Controller
    {
        private ClinicaDbContext db = new ClinicaDbContext();

        // GET: Remedios
        public ActionResult Index()
        {
            return View(db.Remedios.ToList());
        }

        public ActionResult List()
        {
            return View(db.Remedios.ToList());
        }

        // GET: Remedios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remedio remedio = db.Remedios.Find(id);
            if (remedio == null)
            {
                return HttpNotFound();
            }
            return View(remedio);
        }

        // GET: Remedios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Remedios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RemedioId,NomeGenerico,NomeFabrica,Fabricante")] Remedio remedio)
        {
            if (ModelState.IsValid)
            {
                db.Remedios.Add(remedio);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(remedio);
        }

        // GET: Remedios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remedio remedio = db.Remedios.Find(id);
            if (remedio == null)
            {
                return HttpNotFound();
            }
            return View(remedio);
        }

        // POST: Remedios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RemedioId,NomeGenerico,NomeFabrica,Fabricante")] Remedio remedio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(remedio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(remedio);
        }

        // GET: Remedios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Remedio remedio = db.Remedios.Find(id);
            if (remedio == null)
            {
                return HttpNotFound();
            }
            return View(remedio);
        }

        // POST: Remedios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Remedio remedio = db.Remedios.Find(id);
            db.Remedios.Remove(remedio);
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
