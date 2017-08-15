using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LX.NETCoreTest.Model.Models
{
    public partial class PyStudio_NetCoreContext : DbContext
    {
        public virtual DbSet<InfoMember> InfoMember { get; set; }
        public virtual DbSet<ToContent> ToContent { get; set; }
        public virtual DbSet<ToContentFiles> ToContentFiles { get; set; }
        public virtual DbSet<ToModule> ToModule { get; set; }
        public virtual DbSet<ToUserInfo> ToUserInfo { get; set; }
        public virtual DbSet<ToUserLog> ToUserLog { get; set; }

        public PyStudio_NetCoreContext(DbContextOptions<PyStudio_NetCoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InfoMember>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK_Info_Member");

                entity.ToTable("Info_Member");

                entity.Property(e => e.MemberId).HasColumnName("Member_Id");

                entity.Property(e => e.MemberCode)
                    .HasColumnName("Member_Code")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MemberName)
                    .HasColumnName("Member_Name")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ToContent>(entity =>
            {
                entity.ToTable("To_Content");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Des).HasColumnType("varchar(500)");

                entity.Property(e => e.MaxPic).HasColumnType("varchar(200)");

                entity.Property(e => e.MinPic).HasColumnType("varchar(30)");

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.ReadNum).HasDefaultValueSql("0");

                entity.Property(e => e.ZanNum).HasDefaultValueSql("0");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ToContent)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_To_Content_To_Module");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToContent)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_To_Content_To_UserInfo1");
            });

            modelBuilder.Entity<ToContentFiles>(entity =>
            {
                entity.ToTable("To_ContentFiles");

                entity.Property(e => e.MaxPic)
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MinPic).HasColumnType("varchar(30)");

                entity.Property(e => e.ZanNum).HasDefaultValueSql("0");

                entity.HasOne(d => d.Content)
                    .WithMany(p => p.ToContentFiles)
                    .HasForeignKey(d => d.ContentId)
                    .HasConstraintName("FK_To_ContentFiles_To_Content");
            });

            modelBuilder.Entity<ToModule>(entity =>
            {
                entity.ToTable("To_Module");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Des).HasColumnType("varchar(500)");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.SortNum).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<ToUserInfo>(entity =>
            {
                entity.ToTable("To_UserInfo");

                entity.Property(e => e.Addr).HasMaxLength(200);

                entity.Property(e => e.Birthday).HasMaxLength(20);

                entity.Property(e => e.Blog).HasMaxLength(200);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.HeadPhoto).HasMaxLength(200);

                entity.Property(e => e.Introduce).HasMaxLength(200);

                entity.Property(e => e.Ips).HasMaxLength(50);

                entity.Property(e => e.LoginTime).HasColumnType("datetime");

                entity.Property(e => e.NickName).HasMaxLength(20);

                entity.Property(e => e.Tel).HasMaxLength(20);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPwd)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ToUserLog>(entity =>
            {
                entity.ToTable("To_UserLog");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Des).HasColumnType("text");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToUserLog)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_To_UserLog_To_UserInfo");
            });
        }
    }
}