using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICAssessmentFinal.Models
{
    public class ModelBase
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public ModelBase()
        {
            IsActive = true;
        }
    }
}