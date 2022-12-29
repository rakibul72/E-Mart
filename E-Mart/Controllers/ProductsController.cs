using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using E_Mart.Models;

namespace E_Mart.Controllers
{
    public class ProductsController : Controller
    {
        private Entities6 db = new Entities6();

        // GET: Products

       
        public ActionResult Index(string cid)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (cid != null)
            {
                if (cid.Equals("") || cid.Equals("All"))
                {
                    var products = db.Products.Where(u => u.ProductStatus != null);

                    Product product2 = new Product();
                    product2 = (db.Products).FirstOrDefault();
                    return View(products.ToList());
                }
                else
                {
                    //String str = Session["Category"].ToString();
                    string strDDLValue = "1";

                    strDDLValue = cid;


                    //var products = db.Products.Include(p => p.Category);

                    var categoryy = db.Categories.Where(u => u.ProductName.Equals(strDDLValue)).FirstOrDefault();
                    int cc = 0;
                    if (categoryy != null)
                    {
                        cc = categoryy.CategoryID;
                    }
                    var products = db.Products.Where(u => u.CategoryID == cc);
                    Product product2 = new Product();
                    product2 = (db.Products).FirstOrDefault();
                    return View(products.ToList());
                }
            }
            else
            {
                var products = db.Products.Where(u => u.ProductStatus != null);

                Product product2 = new Product();
                product2 = (db.Products).FirstOrDefault();
                return View(products.ToList());
            }
        }


        public ActionResult ApproveProducts(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }
            if (id == null)
            {

                var products = db.Products.Where(u => u.ProductStatus == null);
                Product product2 = new Product();
                product2 = (db.Products).FirstOrDefault();
                return View(products.ToList());
            }
            else
            {
                db.Database.ExecuteSqlCommand("Update Products set ProductStatus = 1  where ProductID = " + id + "");
                db.SaveChanges();


                var products = db.Products.Where(u => u.ProductStatus == null);
                Product product2 = new Product();
                product2 = (db.Products).FirstOrDefault();
                return View(products.ToList());
            }
            
        }

       



        public ActionResult AddToCart(int? productId)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                
                Product product = db.Products.Find(productId);
                int iid = product.ProductID;
                if (Session["cart"] == null)
                {
                    List<Item> cart = new List<Item>();
                   
                    cart.Add(new Item()
                    {
                        ProductID = iid,
                        Quantity = 1
                    });
                    Session["cart"] = cart;
                }
                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];

                    int flag = 0;
                    foreach (var itr in cart)
                    {
                        if (itr.ProductID == iid)
                        {
                            itr.Quantity++;
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        cart.Add(new Item()
                        {
                            ProductID = iid,
                            Quantity = 1
                        });
                    }

                    Session["cart"] = cart;
                }
                return Redirect("../Index");
            }
        }

        public ActionResult Check(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Product product2 = new Product();
            product2 = (db.Products).FirstOrDefault();

            
            int iid = product.ProductID;
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                decimal total = 0;
                total += product.ProductPrice;
                cart.Add(new Item()
                {
                    ProductID = iid,
                    Quantity = 1,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice
                }); ;
                Session["cart"] = cart;
                Session["TotalPrice"] = total;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                decimal total = 0; 
                int flag = 0;

               

                foreach (var itr in cart)
                {
                     
                    if (itr.ProductID == iid)
                    {
                        itr.Quantity++;
                        flag = 1;
                        break;
                    }
                }

                
                if (flag == 0)
                {
                    cart.Add(new Item()
                    {
                        ProductID = iid,
                        Quantity = 1,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice
                    });
                }

                foreach (var itr in cart)
                {

                    total += (itr.ProductPrice * itr.Quantity);
                }

                Session["cart"] = cart;
                Session["TotalPrice"] = total;
            }
            return Redirect("../Index");

            return View(product);
        }


        public ActionResult RemoveItem(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Product product2 = new Product();
            product2 = (db.Products).FirstOrDefault();

            int iid = product.ProductID;
            
                List<Item> cart = (List<Item>)Session["cart"];

            decimal remove = 0;
            decimal temp = 0;
            foreach (var itr in cart)
            {
                remove += (itr.ProductPrice * itr.Quantity);
              

            }
            foreach (var itr in cart)
            {

                    if (itr.ProductID == iid)
                    {
                        temp = (itr.ProductPrice * itr.Quantity);
                    
                         cart.Remove(itr);
                         break;
                    }
                
            }

           
           
            Session["TotalPrice"] = remove-temp;

                

                Session["cart"] = cart;
                
            
            return Redirect("../Cart");

            return View(product);
        }




        /* public ActionResult Index(int? cid)
         {
             var products = db.Products.Include(p => p.Category);
             Product product2 = new Product();
             product2 = (db.Products).FirstOrDefault();
             return View(products.ToList());
         }*/


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            Product product2 = new Product();
            product2 = (db.Products).FirstOrDefault();
           
            
                
            return View(product);
        }

        public ActionResult Logout()
        {




            Session["admin_email"] = null;
            Session["seller_email"] = null;
            Session["buyer_email"] = null;

            Session.Remove("admin_email");
            Session.Remove("seller_email");
            Session.Remove("buyer_email");

            Session.Abandon();
            
            return RedirectToAction("../Home/Index");

            
        }


        public ActionResult Cart()
        {
            //FormsAuthentication.SignOut();
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            return View();
        }

        public ActionResult confirmOrder()
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            List<Item> cart = (List<Item>)Session["cart"];

           
            if (cart!=null)
            {
                foreach (var itr in cart)
                {

                    string str = "";
                    if (Session["buyer_email"] != null)
                        str = Session["buyer_email"].ToString();

                    Has has = db.Hass.Where(u => u.ProductID == itr.ProductID).FirstOrDefault();
                    Buyer buyer = db.Buyers.Where(u => u.BuyerEmail.Equals(str)).FirstOrDefault();
                    int sid = has.SellerID;

                    int aid = 1;
                    int bid = buyer.BuyerID;

                    Order order = new Order();

                    order.SellerID = sid;
                    order.BuyerID = bid;
                    order.ProductID = itr.ProductID;
                    order.AdminID = 1;
                    order.OrderQuantity = itr.Quantity;
                    order.OrderDate = DateTime.Now;
                    order.OrderStatus = 0;
                    db.Orders.Add(order);
                    db.SaveChanges();

                }

                Session["cart"] = null;
                Session["TotalPrice"] = null;
                TempData["Referrer"] = "SaveRegister";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Referrer"] = "Error";
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }
            //FormsAuthentication.SignOut();

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "ProductName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            product.ProductIMG = "~/images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
            product.ImageFile.SaveAs(fileName);

           
                db.Products.Add(product);
                db.SaveChanges();
            
            

            
              

                
                    String str = Session["seller_email"].ToString();

                    var check = db.Sellers.Where(u => u.SellerEmail.Equals(str)).FirstOrDefault();

                    var ID = check.SellerID;

                    var ID2 = product.ProductID;

                    Has hh = new Has();

                    hh.ProductID = product.ProductID;
                    hh.SellerID = ID;
                    db.Hass.Add(hh);
                    db.SaveChanges();

                

                TempData["Referrer"] = "SaveRegister";

                ModelState.Clear();
                return RedirectToAction("Create");
            

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "ProductName", product.CategoryID);
            return View(product);
        }

        [HttpPost]
        

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "ProductName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductDescription,ProductCategory,ProductRating,ProductIMG")] Product product)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "ProductName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            Has hasses = db.Hass.Where(u => u.ProductID == id).FirstOrDefault();
            db.Hass.Remove(hasses);
            db.SaveChanges();
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ApproveProducts");
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
