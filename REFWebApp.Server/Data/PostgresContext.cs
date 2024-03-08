using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using REFWebApp.Server.Models;

namespace REFWebApp.Server.Data;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AudioFile> AudioFiles { get; set; }

    public virtual DbSet<FileFormat> FileFormats { get; set; }

    public virtual DbSet<Metric> Metrics { get; set; }

    public virtual DbSet<Scenario> Scenarios { get; set; }

    public virtual DbSet<Stt> Stts { get; set; }

    public virtual DbSet<SttAggregateMetric> SttAggregateMetrics { get; set; }

    public virtual DbSet<Transcription> Transcriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=ref-data-postgres.cabwik8k5abx.us-east-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=postgres;Password=nasa-team");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudioFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audio_files_pkey");

            entity.ToTable("audio_files");

            entity.HasIndex(e => e.Path, "unique_path").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormatId).HasColumnName("format_id");
            entity.Property(e => e.GroundTruth).HasColumnName("ground_truth");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .HasColumnName("path");

            entity.HasOne(d => d.Format).WithMany(p => p.AudioFiles)
                .HasForeignKey(d => d.FormatId)
                .HasConstraintName("fk_format");

            entity.HasMany(d => d.Scenarios).WithMany(p => p.Audios)
                .UsingEntity<Dictionary<string, object>>(
                    "AudioScenario",
                    r => r.HasOne<Scenario>().WithMany()
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_scenario"),
                    l => l.HasOne<AudioFile>().WithMany()
                        .HasForeignKey("AudioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_audio"),
                    j =>
                    {
                        j.HasKey("AudioId", "ScenarioId").HasName("audio_scenarios_pkey");
                        j.ToTable("audio_scenarios");
                        j.IndexerProperty<int>("AudioId").HasColumnName("audio_id");
                        j.IndexerProperty<int>("ScenarioId").HasColumnName("scenario_id");
                    });
        });

        modelBuilder.Entity<FileFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("file_formats_pkey");

            entity.ToTable("file_formats");

            entity.HasIndex(e => e.Name, "file_formats_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Metric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("metrics_pkey");

            entity.ToTable("metrics");

            entity.HasIndex(e => e.Name, "metrics_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Scenario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("scenarios_pkey");

            entity.ToTable("scenarios");

            entity.HasIndex(e => e.Name, "scenarios_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Stt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stts_pkey");

            entity.ToTable("stts");

            entity.HasIndex(e => e.Name, "stts_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SttAggregateMetric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stt_aggregate_metrics_pkey");

            entity.ToTable("stt_aggregate_metrics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ScenarioId).HasColumnName("scenario_id");
            entity.Property(e => e.SttId).HasColumnName("stt_id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Scenario).WithMany(p => p.SttAggregateMetrics)
                .HasForeignKey(d => d.ScenarioId)
                .HasConstraintName("fk_scenario");

            entity.HasOne(d => d.Stt).WithMany(p => p.SttAggregateMetrics)
                .HasForeignKey(d => d.SttId)
                .HasConstraintName("fk_stt");
        });

        modelBuilder.Entity<Transcription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transcriptions_pkey");

            entity.ToTable("transcriptions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AudioId).HasColumnName("audio_id");
            entity.Property(e => e.SttId).HasColumnName("stt_id");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.Transcript).HasColumnName("transcript");

            entity.HasOne(d => d.Audio).WithMany(p => p.Transcriptions)
                .HasForeignKey(d => d.AudioId)
                .HasConstraintName("fk_audio");

            entity.HasOne(d => d.Stt).WithMany(p => p.Transcriptions)
                .HasForeignKey(d => d.SttId)
                .HasConstraintName("fk_stt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
