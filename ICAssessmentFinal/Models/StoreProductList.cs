using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.Models
{
    public class StoreProductList : ModelBase
    {
        public virtual Store Store { get; set; }
        public virtual Product Product { get; set; }
    }
}