using ECommerce.Classes;
using ECommerce.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "User , Admin")]
    public class WareHousesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();



       

        // GET: WareHouses
        public ActionResult Index()
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var wareHouses = db.WareHouses.Where(w => w.CompanyId == user.CompanyId).Include(w => w.Cities).Include(w => w.Departments);
            return View(wareHouses.ToList());
        }

        // GET: WareHouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WareHouse wareHouse = db.WareHouses.Find(id);
            if (wareHouse == null)
            {
                return HttpNotFound();
            }
            return View(wareHouse);
        }

        // GET: WareHouses/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var ware = new WareHouse
            {
                CompanyId = user.CompanyId
            };

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            
            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name");
            return View(ware);
        }

        // POST: WareHouses/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WareHouse wareHouse)
        {
            if (ModelState.IsValid)
            {
                db.WareHouses.Add(wareHouse);
                var responseSave = DBHelper.SaveChanges(db);
                if (responseSave.Succeeded)
                {
                   
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, responseSave.Message);

            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(wareHouse.DepartamentsId), "CityId", "Name", wareHouse.CityId);
           
            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name", wareHouse.DepartamentsId);
            return View(wareHouse);
        }

        // GET: WareHouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WareHouse wareHouse = db.WareHouses.Find(id);
            if (wareHouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(wareHouse.DepartamentsId), "CityId", "Name", wareHouse.CityId);
          
            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name", wareHouse.DepartamentsId);
            return View(wareHouse);
        }

        // POST: WareHouses/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WareHouse wareHouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wareHouse).State = EntityState.Modified;
                var responseSave = DBHelper.SaveChanges(db);
                if (responseSave.Succeeded)
                {

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, responseSave.Message);


            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(wareHouse.DepartamentsId), "CityId", "Name", wareHouse.CityId);
          
            ViewBag.DepartamentsId = new SelectList(CombosHelper.GetDepartments(), "DepartamentsId", "Name", wareHouse.DepartamentsId);
            return View(wareHouse);
        }

        // GET: WareHouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WareHouse wareHouse = db.WareHouses.Find(id);
            if (wareHouse == null)
            {
                return HttpNotFound();
            }
            return View(wareHouse);
        }

        // POST: WareHouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WareHouse wareHouse = db.WareHouses.Find(id);
            db.WareHouses.Remove(wareHouse);

            var responseSave = DBHelper.SaveChanges(db);
            if (responseSave.Succeeded)
            {

                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, responseSave.Message);
            return View(wareHouse);
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
