using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EllieApi.Model
{
    public partial class ElliedbContext : DbContext
    {
        public ElliedbContext()
        {
        }

        public ElliedbContext(DbContextOptions<ElliedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Alarm> Alarms { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Institute> Institutes { get; set; }

        public virtual DbSet<Note> Notes { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserAlarmRelation> UserAlarmRelations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=DESKTOP-9D9JSER;Database=elliedb;Trusted_Connection=True;Integrated Security=true;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC07B3394DC2");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Alarm>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Alarms__3214EC07648AF227");

                entity.Property(e => e.ActivatingTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07ACE5AC33");

                entity.Property(e => e.Email).HasMaxLength(20);
                entity.Property(e => e.FirstName).HasMaxLength(10);
                entity.Property(e => e.InstituteId).HasColumnName("Institute_id");
                entity.Property(e => e.LastName).HasMaxLength(20);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.Role).HasMaxLength(10);

                entity.HasOne(d => d.Institute).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.InstituteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees.Institute_id");
            });

            modelBuilder.Entity<Institute>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Institut__3214EC07E13FBC42");

                entity.Property(e => e.AddressId).HasColumnName("Address_id");
                entity.Property(e => e.Name).HasMaxLength(20);

                entity.HasOne(d => d.Address).WithMany(p => p.Institutes)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Institutes.Address_id");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Notes__3214EC073CEDC50F");

                entity.Property(e => e.Text)
                    .HasColumnType("text")
                    .HasColumnName("text");
                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.User).WithMany(p => p.Notes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notes.User_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E14B36BD");

                entity.Property(e => e.ContactPersonId).HasColumnName("ContactPerson_id");
                entity.Property(e => e.FirstName).HasMaxLength(10);
                entity.Property(e => e.LastName).HasMaxLength(20);
                entity.Property(e => e.Points).HasDefaultValueSql("('0')");
                entity.Property(e => e.Room).HasMaxLength(10);

                entity.HasOne(d => d.ContactPerson).WithMany(p => p.Users)
                    .HasForeignKey(d => d.ContactPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users.ContactPerson_id");
            });

            modelBuilder.Entity<UserAlarmRelation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__UserAlar__3214EC077F34F957");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AlarmsId).HasColumnName("Alarms_id");
                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Alarms).WithMany(p => p.UserAlarmRelations)
                    .HasForeignKey(d => d.AlarmsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAlarmRelations.Alarms_id");

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.UserAlarmRelation)
                    .HasForeignKey<UserAlarmRelation>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAlarmRelations.Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
