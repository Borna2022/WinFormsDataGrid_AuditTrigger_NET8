using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WinFormsApp2.Models;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TmixedZoneSteelGroupAudit> TmixedZoneSteelGroupAudits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Test;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TmixedZoneSteelGroupAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__TMixedZo__A17F23989E0B69BB");

            entity.ToTable("TMixedZoneSteelGroup_Audit");

            entity.Property(e => e.FsgGroupId).HasColumnName("Fsg_GroupId");
            entity.Property(e => e.FsgRow).HasColumnName("Fsg_Row");
            entity.Property(e => e.OperationTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.PerformedBy).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
