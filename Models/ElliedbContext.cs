using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EllieApi.Models;

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

    public virtual DbSet<AlarmType> AlarmTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Institute> Institutes { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAlarmRelation> UserAlarmRelations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9D9JSER;Database=elliedb;Trusted_Connection=True;Integrated Security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC077D44F74E");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Alarm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Alarms__3214EC0760808E56");

            entity.Property(e => e.ActivatingTime).HasColumnType("datetime");
            entity.Property(e => e.AlarmTypeId).HasColumnName("AlarmType_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(10);

            entity.HasOne(d => d.AlarmType).WithMany(p => p.Alarms)
                .HasForeignKey(d => d.AlarmTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alarms.AlarmType_id");
        });

        modelBuilder.Entity<AlarmType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AlarmTyp__3214EC071E468A4B");

            entity.ToTable("AlarmType");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC077083A692");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(10);
            entity.Property(e => e.InstituteId).HasColumnName("Institute_id");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.PasswordSalt).HasMaxLength(500);
            entity.Property(e => e.RoleId).HasColumnName("Role_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Employees)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees.Institute_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees.Role_id");
        });

        modelBuilder.Entity<Institute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Institut__3214EC074B4B10FA");

            entity.Property(e => e.AddressId).HasColumnName("Address_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Address).WithMany(p => p.Institutes)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Institutes.Address_id");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3214EC076EB2DD32");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notes.User_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07E97E51B6");

            entity.ToTable("Role");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC0754754A15");

            entity.ToTable("Room");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InstituteId).HasColumnName("Institute_id");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Institute).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.InstituteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room.Institute_id");

            entity.HasOne(d => d.User).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Room.User_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC075498799F");

            entity.Property(e => e.ContactPersonId).HasColumnName("ContactPerson_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(10);
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Points).HasDefaultValueSql("('0')");

            entity.HasOne(d => d.ContactPerson).WithMany(p => p.Users)
                .HasForeignKey(d => d.ContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users.ContactPerson_id");
        });

        modelBuilder.Entity<UserAlarmRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAlar__3214EC0754C0614E");

            entity.Property(e => e.AlarmsId).HasColumnName("Alarms_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Alarms).WithMany(p => p.UserAlarmRelations)
                .HasForeignKey(d => d.AlarmsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAlarmRelations.Alarms_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserAlarmRelations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAlarmRelations.User_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
