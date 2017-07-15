using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LX.NETCoreTest.Model.Models
{
    public partial class PyStudio_NetCoreContext : DbContext
    {
        public virtual DbSet<ToUserInfo> ToUserInfo { get; set; }
        public virtual DbSet<ToUserLog> ToUserLog { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=.;User Id=ClearloveLX;Password=GaoKe5845211314;Database=PyStudio.NetCore;");
        //}

        public PyStudio_NetCoreContext(DbContextOptions<PyStudio_NetCoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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