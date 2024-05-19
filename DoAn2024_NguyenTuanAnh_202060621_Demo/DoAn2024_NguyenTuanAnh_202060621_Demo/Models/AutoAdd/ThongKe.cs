namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ThongKe
    {
        public int Id { get; set; }

        public DateTime ThoiGian { get; set; }

        public long SoTruyCap { get; set; }
    }
}
