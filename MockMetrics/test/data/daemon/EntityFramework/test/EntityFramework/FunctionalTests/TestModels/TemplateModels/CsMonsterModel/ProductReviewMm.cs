//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FunctionalTests.ProductivityApi.TemplateModels.CsMonsterModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductReviewMm
    {
        public ProductReviewMm()
        {
            this.Features = new HashSet<ProductWebFeatureMm>();
        }
    
        public int ProductId { get; set; }
        public int ReviewId { get; set; }
        public string Review { get; set; }
    
        public virtual ProductMm Product { get; set; }
        public virtual ICollection<ProductWebFeatureMm> Features { get; set; }
    }
}
