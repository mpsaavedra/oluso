﻿// <auto-generated />
using System;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data.Models.Lecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "createdAt");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "createdBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedAt");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedBy");

                    b.Property<string>("RowVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "rowVersion");

                    b.Property<bool>("Status")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updateAt");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "updatedBy");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("RowVersion")
                        .IsUnique();

                    b.HasIndex("TeacherId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("Data.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "createdAt");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "createdBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedAt");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedBy");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RowVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "rowVersion");

                    b.Property<bool>("Status")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updateAt");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "updatedBy");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Data.Models.StudentLecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LectureId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentLectures");
                });

            modelBuilder.Entity("Data.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "createdAt");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "createdBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedAt");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "deletedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RowVersion")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "rowVersion");

                    b.Property<bool>("Status")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updateAt");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "updatedBy");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("RowVersion")
                        .IsUnique();

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Data.Models.Lecture", b =>
                {
                    b.HasOne("Data.Models.Teacher", "Teacher")
                        .WithMany("Lectures")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Data.Models.StudentLecture", b =>
                {
                    b.HasOne("Data.Models.Lecture", "Lecture")
                        .WithMany("Students")
                        .HasForeignKey("LectureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Student", "Student")
                        .WithMany("Lectures")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecture");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Data.Models.Lecture", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Data.Models.Student", b =>
                {
                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("Data.Models.Teacher", b =>
                {
                    b.Navigation("Lectures");
                });
#pragma warning restore 612, 618
        }
    }
}