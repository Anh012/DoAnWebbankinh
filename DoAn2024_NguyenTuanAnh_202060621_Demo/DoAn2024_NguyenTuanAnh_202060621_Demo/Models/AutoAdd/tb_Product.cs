namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class tb_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Product()
        {
            tb_OrderDetail = new HashSet<tb_OrderDetail>();
            tb_ProductImage = new HashSet<tb_ProductImage>();
            tb_Review = new HashSet<tb_Review>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Description { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public string Detail { get; set; }
        
      
        public string Image { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin")]
        public decimal Price { get; set; }

        public decimal? PriceSale { get; set; }

        public int Quantity { get; set; }

        public bool IsHome { get; set; }

        public bool IsSale { get; set; }

        public bool IsFeature { get; set; }

        public bool IsHot { get; set; }

        public int ProductCategoryId { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }

        [StringLength(500)]
        public string SeoDescription { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Modifiedby { get; set; }

        [StringLength(250)]
        public string Alias { get; set; }

        public bool IsActive { get; set; }

        public int ViewCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_OrderDetail> tb_OrderDetail { get; set; }

        public virtual tb_ProductCategory tb_ProductCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ProductImage> tb_ProductImage { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_Review> tb_Review { get; set; }
    }
}
