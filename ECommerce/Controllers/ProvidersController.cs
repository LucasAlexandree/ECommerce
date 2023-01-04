using ECommerce.Classes;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static ECommerce.Classes.UserHelper;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class ProvidersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Providers
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var qry = (from pr in db.Providers
                       join cp in db.CompanyProviders on pr.ProviderId equals cp.ProviderId
                       join co in db.Companies on cp.CompanyId equals co.CompanyId
                       where co.CompanyId == user.CompanyId
                       select new { pr }).ToList();



            var provider = new List<Provider>();
            foreach (var item in qry)
            {
                provider.Add(item.pr);
            }
           
            return View(provider.ToList());
        }

        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");

            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
            return View();
        }

        // POST: Providers/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Providers.Add(provider);
                        var responseSave = DBHelper.SaveChanges(db);
                        if (!responseSave.Succeeded)
                        {

                            ModelState.AddModelError(string.Empty, responseSave.Message);
                            transaction.Rollback();
                            ViewBag.CityId = new SelectList(CombosHelper.GetCities(provider.DepartamentsId), "CityId", "Name");

                            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
                            return View(provider);
                        }

                        UsersHelper.CreateUserASP(provider.UserName, "Provider");


                        //SALVAR OS DADOS NA TABELA COMPANY PROVIDER
                        var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                        var companyProvider = new CompanyProvider()
                        {
                            CompanyId = user.CompanyId,
                            ProviderId = provider.ProviderId
                        };

                        db.CompanyProviders.Add(companyProvider);
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(provider.DepartamentsId), "CityId", "Name");

            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(provider.DepartamentsId), "CityId", "Name");

            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
            return View(provider);
        }

        // POST: Providers/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(provider.DepartamentsId), "CityId", "Name");

            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var provider = db.Providers.Find(id);
            var companyProvider = db.CompanyProviders.Where(cc => cc.CompanyId == user.CompanyId && cc.ProviderId == provider.ProviderId).FirstOrDefault();

            using (var transaction = db.Database.BeginTransaction())
            {

                db.CompanyProviders.Remove(companyProvider);
                db.Providers.Remove(provider);

                var responseSave = DBHelper.SaveChanges(db);
                if (responseSave.Succeeded)
                {
                    transaction.Commit();
                    return RedirectToAction("Index");

                }

                ModelState.AddModelError(string.Empty, responseSave.Message);
                transaction.Rollback();
                return View(provider);
            }
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
