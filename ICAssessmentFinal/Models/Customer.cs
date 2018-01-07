using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.Models
{
    public class Customer : ModelBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}