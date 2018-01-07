using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.Models
{
    public enum ProductCategoryEnum
    {
        [Display(Name = "Food")]
        Food,

        [Display(Name = "Cloth")]
        Cloth,

        [Display(Name = "Book")]
        Book
    }

    public class Product : ModelBase
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public ProductCategoryEnum Category { get; set; }
        public virtual ICollection<Store> StoreList { get; set; }
    }
}