using ICAssessmentFinal.Common;
using ICAssessmentFinal.Models;
using ICAssessmentFinal.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ICAssessmentFinal.Controllers
{
    public class StoreController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        EditableTableOption storeTableOption;
        EditableTableOption storeProductTableOption; 

        public StoreController()
        {
            List<Tuple<string, string>> storePropertyList = new List<Tuple<string, string>>();
            Type t = typeof(Store);
            MemberInfo[] members = t.GetProperties();
            var result = from m in members
                         where m.Name != "Id" && m.Name != "ProductList"
                         select m.ToString();
            foreach (var r in result)
            {
                string[] NameAndType = r.Split(' ');
                string columnName = NameAndType[1];
                string elementString = KCHandler.ConvertBindTypeFromDataType(columnName, NameAndType[0].Split('.').Last());
                storePropertyList.Add(new Tuple<string, string>(columnName, elementString));
            }
            storeTableOption = new EditableTableOption(storePropertyList);

            List<Tuple<string, string>> storeProductPropertyList = new List<Tuple<string, string>>();
            storeProductPropertyList.Add(new Tuple<string, string>("ProductName", KCHandler.ConvertBindTypeFromDataType("ProductName", "", true)));
            storeProductPropertyList.Add(new Tuple<string, string>("IsAvailable", KCHandler.ConvertBindTypeFromDataType("IsAvailable", "Boolean")));
            storeProductTableOption = new EditableTableOption(storeProductPropertyList, false, false);
        }
        // GET: Store
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(storeTableOption);
        }

        [HttpGet]
        public JsonResult GetList()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            var stores = from s in db.Stores
                         select new { Id = s.Id, Name = s.Name, Address = s.Address, IsActive = s.IsActive };

            return Json(stores.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNameList()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            var stores = from s in db.Stores
                         where s.IsActive == true
                         select new { Id = s.Id, Name = s.Name};

            return Json(stores.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStore(Store store)
        {
            string result = "";
            var target = db.Stores.Single(x => x.Id == store.Id);
            target.Name = store.Name;
            target.Address = store.Address;

            try
            {
                db.SaveChanges();
                result = "New record has been added";
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }

            return new ContentResult() { Content = result };
        }

        public class StoreViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public bool IsActive { get; set; }
            public bool updated { get; set; }
            public bool _destroy { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Save(IEnumerable<StoreViewModel> stores)
        {
            foreach (var store in stores)
            {
                if (store._destroy)
                {
                    Store target = db.Stores.Find(store.Id);
                    if (target != null)
                    {
                        db.Stores.Remove(target);
                    }
                }
                else if (store.Id == 0) //New record
                {
                    db.Stores.Add(new Store() { Name = store.Name, Address = store.Address });
                }
                else if (store.updated)
                {
                    Store target = db.Stores.Find(store.Id);
                    if (target != null)
                    {
                        target.Name = store.Name;
                        target.Address = store.Address;
                        target.IsActive = store.IsActive;
                    }
                }
            }
            db.SaveChanges();


            return new ContentResult() { Content = "Saved" };
        }

        //================================================================
        // Store Product List
        //================================================================
        [Authorize(Roles = "Admin")]
        public ActionResult ProductList()
        {
            return View(storeProductTableOption);
        }

        [HttpGet]
        public ActionResult GetProductList(int storeId)
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            var store = db.Stores.Find(storeId);
            var productList = from p in db.Products
                           where p.IsActive == true
                           select new StoreProductListViewModel() { Id = p.Id, ProductName = p.Name, IsAvailable = (p.StoreList.Any(s => s.Id == storeId)) };

            return Json(productList.ToList(), JsonRequestBehavior.AllowGet);
        }

        public class StoreProductViewModel
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public bool IsAvailable { get; set; }
            public bool updated { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveStoreProductList(int storeId, IEnumerable<StoreProductViewModel> productList)
        {
            var store = db.Stores.Find(storeId);
            if (store == null)
            {
                return Content("WTH??");
            }
            foreach (var product in productList)
            {
                if (product.updated)
                {
                    var targetProduct = db.Products.Find(product.Id);
                    if (product.IsAvailable)
                    {
                        store.ProductList.Add(targetProduct);
                    }
                    else
                    {
                        store.ProductList.Remove(targetProduct);
                    }
                    db.SaveChanges();
                }
            }
            return Content("Saved");
        }
    }
}