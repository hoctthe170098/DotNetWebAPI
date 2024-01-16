using Microsoft.EntityFrameworkCore;

namespace WebAPI1.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        #region DbSet
        public DbSet<HangHoa> HangHoas { get; set;}
        public DbSet<Loai>Loais { get; set;}
        public DbSet<DonHang> DonHangs { get; set;}
        public DbSet<DonHangChiTiet>DonHangChiTiets { get; set;}
        public DbSet<NguoiDung>NguoiDungs { get; set;}
        public DbSet<RefreshToken> RefreshTokens { get; set;}
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHang");
                e.HasKey(dh=>dh.MaDh);
                e.Property(dh=>dh.NgayDat).HasDefaultValueSql("getutcdate()");
                e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHang");
                e.HasKey(d => new { d.MaDh, d.MaHH });
                e.HasOne(d => d.DonHang).WithMany(d => d.DonHangChiTiets).HasForeignKey(d => d.MaDh);
                e.HasOne(d => d.HangHoa).WithMany(d => d.DonHangChiTiets).HasForeignKey(d => d.MaHH);
            });
            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}
