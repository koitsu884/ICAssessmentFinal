using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.ViewModels.Store
{
    public class StoreProductListViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public bool IsAvailable { get; set; }
    }
}