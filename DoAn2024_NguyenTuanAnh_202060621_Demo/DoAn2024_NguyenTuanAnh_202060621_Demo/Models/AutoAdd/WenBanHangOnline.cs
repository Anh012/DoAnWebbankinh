using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAn2024_NguyenTuanAnh_202060621_Demo.Models.AutoAdd
{
    public partial class WenBanHangOnline : DbContext
    {
        public WenBanHangOnline()
            : base("name=WenBanHangOnline")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tb_Adv> tb_Adv { get; set; }
        public virtual DbSet<tb_Category> tb_Category { get; set; }
        public virtual DbSet<tb_Contact> tb_Contact { get; set; }
        public virtual DbSet<tb_News> tb_News { get; set; }
        public virtual DbSet<tb_Order> tb_Order { get; set; }
        public virtual DbSet<tb_OrderDetail> tb_OrderDetail { get; set; }
        public virtual DbSet<tb_Posts> tb_Posts { get; set; }
        public virtual DbSet<tb_Product> tb_Product { get; set; }
        public virtual DbSet<tb_ProductCategory> tb_ProductCategory { get; set; }
        public virtual DbSet<tb_ProductImage> tb_ProductImage { get; set; }
        public virtual DbSet<tb_Subscribe> tb_Subscribe { get; set; }
        public virtual DbSet<tb_SystemSetting> tb_SystemSetting { get; set; }
        public virtual DbSet<ThongKe> ThongKes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserOder> UserOders { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<tb_Review> tb_Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.idRole);

            modelBuilder.Entity<tb_Category>()
                .HasMany(e => e.tb_News)
                .WithRequired(e => e.tb_Category)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<tb_Category>()
                .HasMany(e => e.tb_Posts)
                .WithRequired(e => e.tb_Category)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<tb_Order>()
                .HasMany(e => e.tb_OrderDetail)
                .WithRequired(e => e.tb_Order)
                .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<tb_Order>()
                .HasMany(e => e.UserOders)
                .WithRequired(e => e.tb_Order)
                .HasForeignKey(e => e.IdOder);

            modelBuilder.Entity<tb_Product>()
                .HasMany(e => e.tb_OrderDetail)
                .WithRequired(e => e.tb_Product)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<tb_Product>()
                .HasMany(e => e.tb_ProductImage)
                .WithRequired(e => e.tb_Product)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<tb_Product>()
              .HasMany(e => e.tb_Review)
              .WithRequired(e => e.tb_Product)
              .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<tb_ProductCategory>()
                .HasMany(e => e.tb_Product)
                .WithRequired(e => e.tb_ProductCategory)
                .HasForeignKey(e => e.ProductCategoryId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserOders)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IdUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.idUser);
        }
    }
}
