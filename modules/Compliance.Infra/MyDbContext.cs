using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DocumentUpload.Service;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<TdhcomplianceDocument> TdhcomplianceDocuments { get; set; }

    public virtual DbSet<Tdhdocument> Tdhdocuments { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-H0J6VR6K\\SQLEXPRESS;Database=ktdh;TrustServerCertificate=True;Integrated Security=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       

        modelBuilder.Entity<TdhcomplianceDocument>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TDHComplianceDocuments");

            entity.Property(e => e.ComplianceId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.DocumentId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.EntityId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FileName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Tdhdocument>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TDHDocuments");
            entity.HasKey(e => e.Id)
                .HasName("PK_ID")
                .HasFillFactor(70);
            entity.Property(e => e.DocumentId)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.EntityId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.GroupId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModuleId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UploadedDate)
                .HasMaxLength(500)
                .IsFixedLength();
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
