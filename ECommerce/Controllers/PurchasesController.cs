using ECommerce.Classes;
using ECommerce.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class PurchasesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();



        public ActionResult InventoryValidate()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = db.Products.Include(p => p.Category).
                Include(p => p.Tax).
                Where(c => c.CompanyId == user.CompanyId);

            return View(products.ToList());
        }


        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var purchaseDetailTmps = db.PurchaseDetailTmps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductId == id).FirstOrDefault();

            if (purchaseDetailTmps == null)
            {
                return HttpNotFound();
            }
            db.PurchaseDetailTmps.Remove(purchaseDetailTmps);

            var quantity = purchaseDetailTmps.Quantity;
            var inventory = db.Inventories.Where(i => i.ProductId == purchaseDetailTmps.ProductId).FirstOrDefault();
            inventory.Stock -= quantity;

            db.SaveChanges();
            return RedirectToAction("Create");
        }


        //ADD PRODUTOS GET
        public ActionResult AddProduct()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId, true), "ProductId", "Description");
            ViewBag.WareHouseId = new SelectList(CombosHelper.GetWareHouse(user.CompanyId, true), "WareHouseId", "Name");
            return PartialView();
        }



        //ADD PRODUTOS POST
        [HttpPost]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var purchaseDetailTmps = db.PurchaseDetailTmps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductId == view.ProductId).FirstOrDefault();
                if (purchaseDetailTmps == null)
                {

                    var product = db.Products.Find(view.ProductId);
                    purchaseDetailTmps = new PurchaseDetailTmp
                    {
                        Description = product.Description,
                        Price = product.Price,
                        ProductId = product.ProductId,
                        Quantity = view.Quantity,
                        TaxRate = product.Tax.Rate,
                        UserName = User.Identity.Name,

                    };

                    db.PurchaseDetailTmps.Add(purchaseDetailTmps);


                    var products = db.Products.Find(view.ProductId);
                    var inventories = new Inventory
                    {

                        ProductId = product.ProductId,
                        WareHouseId = view.WareHouseId,
                        Stock = 0

                    };

                    db.Inventories.Add(inventories);
                    db.SaveChanges();

                }
                else
                {
                    purchaseDetailTmps.Quantity += view.Quantity;
                    db.Entry(purchaseDetailTmps).State = EntityState.Modified;

                }

                


                var quantity = view.Quantity;
                var inventory = db.Inventories.Where(i => i.ProductId == view.ProductId).FirstOrDefault();
                inventory.Stock += quantity;

                if (purchaseDetailTmps.Product.Stock <= 50)
                {
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("InventoryValidate");
                }
            }

            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId), "ProductId", "Description");
            ViewBag.WareHouseId = new SelectList(CombosHelper.GetWareHouse(user.CompanyId, true), "WareHouseId", "Name");
            return PartialView();
        }








        // GET: Purchases
        public ActionResult Index()
        {
            var purchase = db.Purchase.Include(p => p.Company).Include(p => p.Provider).Include(p => p.State);
            return View(purchase.ToList());
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProviderId = new SelectList(CombosHelper.GetProvider(user.CompanyId), "ProviderId", "FullName");
            var view = new NewPurchaseView
            {
                Date = DateTime.Now,
                Details = db.PurchaseDetailTmps.Where(odt => odt.UserName == User.Identity.Name).ToList(),
            };

            return View(view);
        }

        // POST: Purchases/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewPurchaseView view)
        {
            if (ModelState.IsValid)
            {
                var response = MovementsHelper.NewPurchase(view, User.Identity.Name);
                if (response.Succeeded)
                {
                                    
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }


            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProviderId = new SelectList(CombosHelper.GetProvider(user.CompanyId), "ProviderId", "FullName", view.ProviderId);
            view.Details = db.PurchaseDetailTmps.Where(odt => odt.UserName == User.Identity.Name).ToList();
            return View(view);
        }

        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", purchase.CompanyId);
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "UserName", purchase.ProviderId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", purchase.StateId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseId,ProviderId,CompanyId,StateId,Date,Remarks")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", purchase.CompanyId);
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "UserName", purchase.ProviderId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", purchase.StateId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Purchase purchase = db.Purchase.Find(id);
            db.Purchase.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
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
