

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class tb_Review
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
         
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Emaill { get; set; }
        public string Contentt { get; set;}
        public string Rate { get; set;}
        public DateTime CreatedDate { get; set; }
        public virtual tb_Product tb_Product { get; set; }

    }
}