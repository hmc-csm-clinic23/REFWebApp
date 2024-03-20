﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using REFWebApp.Server.Data;

#nullable disable

namespace REFWebApp.Server.Data.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20240305032709_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AudioScenario", b =>
                {
                    b.Property<int>("AudioId")
                        .HasColumnType("integer")
                        .HasColumnName("audio_id");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("integer")
                        .HasColumnName("scenario_id");

                    b.HasKey("AudioId", "ScenarioId")
                        .HasName("audio_scenarios_pkey");

                    b.HasIndex("ScenarioId");

                    b.ToTable("audio_scenarios", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.AudioFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FormatId")
                        .HasColumnType("integer")
                        .HasColumnName("format_id");

                    b.Property<string>("GroundTruth")
                        .HasColumnType("text")
                        .HasColumnName("ground_truth");

                    b.Property<string>("Path")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("path");

                    b.HasKey("Id")
                        .HasName("audio_files_pkey");

                    b.HasIndex("FormatId");

                    b.ToTable("audio_files", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.FileFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("file_formats_pkey");

                    b.HasIndex(new[] { "Name" }, "file_formats_name_key")
                        .IsUnique();

                    b.ToTable("file_formats", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("metrics_pkey");

                    b.HasIndex(new[] { "Name" }, "metrics_name_key")
                        .IsUnique();

                    b.ToTable("metrics", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Scenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("scenarios_pkey");

                    b.HasIndex(new[] { "Name" }, "scenarios_name_key")
                        .IsUnique();

                    b.ToTable("scenarios", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Stt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("stts_pkey");

                    b.HasIndex(new[] { "Name" }, "stts_name_key")
                        .IsUnique();

                    b.ToTable("stts", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.SttAggregateMetric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ScenarioId")
                        .HasColumnType("integer")
                        .HasColumnName("scenario_id");

                    b.Property<int?>("SttId")
                        .HasColumnType("integer")
                        .HasColumnName("stt_id");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("stt_aggregate_metrics_pkey");

                    b.HasIndex("ScenarioId");

                    b.HasIndex("SttId");

                    b.ToTable("stt_aggregate_metrics", (string)null);
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Transcription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AudioId")
                        .HasColumnType("integer")
                        .HasColumnName("audio_id");

                    b.Property<int?>("SttId")
                        .HasColumnType("integer")
                        .HasColumnName("stt_id");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("transcriptions_pkey");

                    b.HasIndex("AudioId");

                    b.HasIndex("SttId");

                    b.ToTable("transcriptions", (string)null);
                });

            modelBuilder.Entity("AudioScenario", b =>
                {
                    b.HasOne("REFWebApp.Server.Models.AudioFile", null)
                        .WithMany()
                        .HasForeignKey("AudioId")
                        .IsRequired()
                        .HasConstraintName("fk_audio");

                    b.HasOne("REFWebApp.Server.Models.Scenario", null)
                        .WithMany()
                        .HasForeignKey("ScenarioId")
                        .IsRequired()
                        .HasConstraintName("fk_scenario");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.AudioFile", b =>
                {
                    b.HasOne("REFWebApp.Server.Models.FileFormat", "Format")
                        .WithMany("AudioFiles")
                        .HasForeignKey("FormatId")
                        .HasConstraintName("fk_format");

                    b.Navigation("Format");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.SttAggregateMetric", b =>
                {
                    b.HasOne("REFWebApp.Server.Models.Scenario", "Scenario")
                        .WithMany("SttAggregateMetrics")
                        .HasForeignKey("ScenarioId")
                        .HasConstraintName("fk_scenario");

                    b.HasOne("REFWebApp.Server.Models.Stt", "Stt")
                        .WithMany("SttAggregateMetrics")
                        .HasForeignKey("SttId")
                        .HasConstraintName("fk_stt");

                    b.Navigation("Scenario");

                    b.Navigation("Stt");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Transcription", b =>
                {
                    b.HasOne("REFWebApp.Server.Models.AudioFile", "Audio")
                        .WithMany("Transcriptions")
                        .HasForeignKey("AudioId")
                        .HasConstraintName("fk_audio");

                    b.HasOne("REFWebApp.Server.Models.Stt", "Stt")
                        .WithMany("Transcriptions")
                        .HasForeignKey("SttId")
                        .HasConstraintName("fk_stt");

                    b.Navigation("Audio");

                    b.Navigation("Stt");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.AudioFile", b =>
                {
                    b.Navigation("Transcriptions");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.FileFormat", b =>
                {
                    b.Navigation("AudioFiles");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Scenario", b =>
                {
                    b.Navigation("SttAggregateMetrics");
                });

            modelBuilder.Entity("REFWebApp.Server.Models.Stt", b =>
                {
                    b.Navigation("SttAggregateMetrics");

                    b.Navigation("Transcriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
