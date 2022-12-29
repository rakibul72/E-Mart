using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Mart.Models;

namespace E_Mart.Controllers
{
    public class HasesController : Controller
    {
        private Entities6 db = new Entities6();

        // GET: Hases
        public ActionResult Index()
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }
            var hass = db.Hass.Include(h => h.Product).Include(h => h.Seller);
            return View(hass.ToList());
        }

        // GET: Hases/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Has has = db.Hass.Find(id);
            if (has == null)
            {
                return HttpNotFound();
            }
            return View(has);
        }

        // GET: Hases/Create

        public ActionResult viewProducts(int? id)
        {

            /* Order orders = db.Orders.Where(u => u.ProductID == 1).FirstOrDefault();
             db.Orders.Remove(orders);
             db.SaveChanges();*/
            /*Product product = db.Products.Where(u => u.ProductID == 1).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();*/


            /* Order orders2 = db.Orders.Where(u => u.ProductID == 2).FirstOrDefault();

             while (orders2 != null)
             {
                 db.Orders.Remove(orders2);
                 db.SaveChanges();

                 orders2 = db.Orders.Where(u => u.ProductID == 2).FirstOrDefault();
             }*/
            /*Product product2 = db.Products.Where(u => u.ProductID == 2).FirstOrDefault();
            db.Products.Remove(product2);
            db.SaveChanges();*/

            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string str = "";
            if (Session["seller_email"] != null)
                str = Session["seller_email"].ToString();
            Seller sellers = db.Sellers.Where(u => u.SellerEmail.Equals(str)).FirstOrDefault();
            id = sellers.SellerID;

            if (id == null)
            {


                return RedirectToAction("../Sellers/LogIn");
            }
            else
            {



                var has = db.Hass.Where(u => u.SellerID == id);
                Has has2 = new Has();
                has2 = (db.Hass).FirstOrDefault();
                return View(has.ToList());
            }

        }
        public ActionResult Create()
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName");
            return View();
        }

        // POST: Hases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HasID,SellerID,ProductID")] Has has)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (ModelState.IsValid)
            {
                db.Hass.Add(has);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", has.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", has.SellerID);
            return View(has);
        }

        // GET: Hases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Has has = db.Hass.Find(id);
            if (has == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", has.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", has.SellerID);
            return View(has);
        }

        // POST: Hases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HasID,SellerID,ProductID")] Has has)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (ModelState.IsValid)
            {
                db.Entry(has).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", has.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", has.SellerID);
            return View(has);
        }

        // GET: Hases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Has has = db.Hass.Find(id);

            

            if (has == null)
            {
                return HttpNotFound();
            }
            return View(has);
        }

        // POST: Hases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["seller_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            Has has = db.Hass.Where(u => u.HasID == id).FirstOrDefault();
            int pid = has.ProductID;


           

            Has has2 = db.Hass.Find(id);
            db.Hass.Remove(has2);
            db.SaveChanges();

            Order orders2 = db.Orders.Where(u => u.ProductID == pid).FirstOrDefault();

            while (orders2 != null)
            {
                db.Orders.Remove(orders2);
                db.SaveChanges();

                orders2 = db.Orders.Where(u => u.ProductID == pid).FirstOrDefault();
            }

            Product product = db.Products.Where(u => u.ProductID == pid).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();


            return RedirectToAction("viewProducts");
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
