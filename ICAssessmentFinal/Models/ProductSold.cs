using System;

namespace ICAssessmentFinal.Models
{
    public class ProductSold : ModelBase
    {
        public DateTime DateSold { get; set; }

        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }

        /*    [Key]
            public string ApplicationUserId { get; set; }
            [ForeignKey("ApplicationUserId")]
            public virtual ApplicationUser User {get;set;}*/
    }
}