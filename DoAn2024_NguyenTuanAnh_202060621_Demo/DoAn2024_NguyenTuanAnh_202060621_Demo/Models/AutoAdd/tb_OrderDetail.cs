namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual tb_Order tb_Order { get; set; }

        public virtual tb_Product tb_Product { get; set; }
    }
}
