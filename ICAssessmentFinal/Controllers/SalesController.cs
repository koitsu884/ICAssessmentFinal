using ICAssessmentFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICAssessmentFinal.Controllers
{
    public class SalesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Sales
        public ActionResult Index()
        {
            var activeProducts = from p in db.Products
                                 where p.IsActive == true
                                 select p;
            return View(activeProducts.ToList());
        }

        [Authorize(Roles ="Admin")]
        public ActionResult SalesList()
        {
            var salesList = from sl in db.ProductSolds
                            where sl.IsActive == true
                            select sl;
            return View(salesList.ToList());
        }

        [HttpGet]
        public ActionResult GetProductList(int id)
        {
            ViewBag.StoreId = id;
            var store = db.Stores.Find(id);
            if(store == null)
            {
                var activeProducts = from p in db.Products
                                     where p.IsActive == true
                                     select p;
                return PartialView("_ProductList", activeProducts.ToList());
            }
            var productList = from pl in store.ProductList
                              where pl.IsActive == true
                              select pl;
            ViewBag.StoreName = store.Name;
            return PartialView("_ProductList", productList.ToList());
        }

        [HttpGet]
        public ActionResult ProductDetails(int? id, int? storeId, string storeName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = storeId;
            ViewBag.Storename = storeName;
            return View(product);
        }

        [Authorize]
        public ActionResult Purchase(int productId, int storeId, string userName)
        {
            var product = db.Products.Find(productId);
            var store = db.Stores.Find(storeId);
            var customer = (from c in db.Customers
                           where c.ApplicationUser.UserName == userName
                           select c).First();

            db.ProductSolds.Add(new ProductSold() { Product = product, Customer = customer, Store = store, DateSold = DateTime.Now });
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                ViewBag.Result = ex.Message;
                return View();
            }
            ViewBag.Result = "Successfully Saved";
            return View();
        }
    }
}