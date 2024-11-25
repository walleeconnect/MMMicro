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
            entity.HasKey(e => e.Id)
                .HasName("PK_ID")
                .HasFillFactor(70);
            entity.Property(e => e.ComplianceId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.CreatedDate)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.DocumentId)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.EntityId)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.Status)
             .HasMaxLength(100)
             .IsFixedLength();
            entity.Property(e => e.IsActive)
            .HasMaxLength(1)
            .IsFixedLength();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
