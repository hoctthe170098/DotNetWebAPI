﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI1.Data;

#nullable disable

namespace WebAPI1.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240108032632_RefreshToken")]
    partial class RefreshToken
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebAPI1.Data.DonHang", b =>
                {
                    b.Property<Guid>("MaDh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiaChiGiao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayDat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<DateTime?>("NgayGiao")
                        .HasColumnType("datetime2");

                    b.Property<string>("NguoiNhan")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SoDienthoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TinhTrangDonHang")
                        .HasColumnType("int");

                    b.HasKey("MaDh");

                    b.ToTable("DonHang", (string)null);
                });

            modelBuilder.Entity("WebAPI1.Data.DonHangChiTiet", b =>
                {
                    b.Property<Guid>("MaDh")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MaHH")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("DonGia")
                        .HasColumnType("float");

                    b.Property<byte>("GiamGia")
                        .HasColumnType("tinyint");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaDh", "MaHH");

                    b.HasIndex("MaHH");

                    b.ToTable("ChiTietDonHang", (string)null);
                });

            modelBuilder.Entity("WebAPI1.Data.HangHoa", b =>
                {
                    b.Property<Guid>("MaHH")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("DonGia")
                        .HasColumnType("float");

                    b.Property<byte>("GiamGia")
                        .HasColumnType("tinyint");

                    b.Property<int?>("MaLoai")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenHH")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaHH");

                    b.HasIndex("MaLoai");

                    b.ToTable("HangHoa");
                });

            modelBuilder.Entity("WebAPI1.Data.Loai", b =>
                {
                    b.Property<int>("MaLoai")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaLoai"));

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaLoai");

                    b.ToTable("Loai");
                });

            modelBuilder.Entity("WebAPI1.Data.NguoiDung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("NguoiDung");
                });

            modelBuilder.Entity("WebAPI1.Data.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Exprired")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("WebAPI1.Data.DonHangChiTiet", b =>
                {
                    b.HasOne("WebAPI1.Data.DonHang", "DonHang")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaDh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI1.Data.HangHoa", "HangHoa")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaHH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonHang");

                    b.Navigation("HangHoa");
                });

            modelBuilder.Entity("WebAPI1.Data.HangHoa", b =>
                {
                    b.HasOne("WebAPI1.Data.Loai", "Loai")
                        .WithMany("HangHoas")
                        .HasForeignKey("MaLoai");

                    b.Navigation("Loai");
                });

            modelBuilder.Entity("WebAPI1.Data.RefreshToken", b =>
                {
                    b.HasOne("WebAPI1.Data.NguoiDung", "NguoiDung")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("WebAPI1.Data.DonHang", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebAPI1.Data.HangHoa", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebAPI1.Data.Loai", b =>
                {
                    b.Navigation("HangHoas");
                });
#pragma warning restore 612, 618
        }
    }
}
