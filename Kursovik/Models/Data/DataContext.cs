using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik.Models.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base()
        {
        }

        public virtual DbSet<Discipline> Disciplines { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherType> TeacherTypes { get; set; }
        public virtual DbSet<TeacherDiscipline> TeacherDisciplines { get; set; }
        public virtual DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\user\\Desktop\\3 курс\\Kursovik\\DataBase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Room).HasColumnName("Room");
                entity.Property(e => e.Comment).HasColumnName("Comment");
                entity.Property(e => e.StartDate).HasColumnName("StartDate");
                entity.Property(e => e.EndDate).HasColumnName("EndDate");
                entity.Property(e => e.Time).HasColumnName("Hours");
                entity.Property(e => e.PositionId).HasColumnName("Position_Id");

                entity.HasOne(d => d.Position).WithMany(p => p.Disciplines)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FullName).HasColumnName("FullName");
                entity.Property(e => e.Phone).HasColumnName("Phone");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Photo_path).HasColumnName("Photo_Path");
                entity.Property(e => e.PositionId).HasColumnName("Position_id");
                entity.Property(e => e.TypeId).HasColumnName("Type_id");
                entity.Property(e => e.Login).HasColumnName("Login");
                entity.Property(e => e.Password).HasColumnName("Password");

                entity.HasOne(d => d.Position).WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TeacherType).WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeacherDiscipline>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.ToTable("Teacher_Discipline");

                entity.Property(e => e.TeacherId).HasColumnName("Teacher_id");
                entity.Property(e => e.DisciplineId).HasColumnName("Discipline_id");

                entity.HasOne(d => d.Teachers).WithMany(p => p.TeacherDisciplines)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Discipline).WithMany(p => p.TeacherDisciplines)
                    .HasForeignKey(d => d.DisciplineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeacherType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Teacher_Type");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.TypeName).HasColumnName("Type_Name");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
