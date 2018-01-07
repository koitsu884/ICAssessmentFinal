using ICAssessmentFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ICAssessmentFinal.Common;

namespace ICAssessmentFinal.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        EditableTableOption storeTableOption;

        public ProductController()
        {
            List<Tuple<string, string>> productPropertyList = new List<Tuple<string, string>>();
            Type t = typeof(Product);
            MemberInfo[] members = t.GetProperties();
            var result = from m in members
                         where m.Name != "Id" && m.Name != "StoreList"
                         select m.ToString();
            foreach (var r in result)
            {
                string[] NameAndType = r.Split(' ');
                string columnName = NameAndType[1];
                string elementString = KCHandler.ConvertBindTypeFromDataType(columnName, NameAndType[0].Split('.').Last());
                productPropertyList.Add(new Tuple<string,string>(columnName, elementString));
            }
            storeTableOption = new EditableTableOption(productPropertyList);
        }
        // GET: Product
        [Authorize(Roles ="Admin")]
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

            var products = from p in db.Products
                         select new { Id = p.Id, Name = p.Name, Price = p.Price, IsActive = p.IsActive, Category = ((ProductCategoryEnum)p.Category).ToString() };

            return Json(products.ToList(), JsonRequestBehavior.AllowGet);
        }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
            public bool IsActive { get; set; }
            public bool updated { get; set; }
            public bool _destroy { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Save(IEnumerable<ProductViewModel> products)
        {
            foreach (var product in products)
            {
                if (product._destroy)
                {
                    Product target = db.Products.Find(product.Id);
                    if (target != null)
                    {
                        db.Products.Remove(target);
                    }
                }
                else if (product.Id == 0) //New record
                {
                    if(!Enum.TryParse(product.Category, out ProductCategoryEnum convertedCategory))
                    {
                        convertedCategory = ProductCategoryEnum.Food;
                    }
                    db.Products.Add(new Product() { Name = product.Name, Price = product.Price, Category = convertedCategory });
                }
                else if (product.updated)
                {
                    Product target = db.Products.Find(product.Id);
                    if (target != null)
                    {
                        target.Name = product.Name;
                        target.Price = product.Price;
                        if( Enum.TryParse(product.Category, out ProductCategoryEnum convertedCategory))
                        {
                            target.Category = convertedCategory;
                        }
                        target.IsActive = product.IsActive;
                    }
                }
            }
            db.SaveChanges();


            return new ContentResult() { Content = "Saved" };
        }

        [HttpGet]
        public JsonResult GetProductEnumArray()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            return Json(Enum.GetNames(typeof(ProductCategoryEnum)).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}