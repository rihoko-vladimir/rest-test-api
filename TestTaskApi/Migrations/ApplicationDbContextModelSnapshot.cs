﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestTaskApi.Models.DbContext;

#nullable disable

namespace TestTaskApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EmployeeJobTitle", b =>
                {
                    b.Property<Guid>("EmployeesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("JobTitlesId")
                        .HasColumnType("char(36)");

                    b.HasKey("EmployeesId", "JobTitlesId");

                    b.HasIndex("JobTitlesId");

                    b.ToTable("EmployeeJobTitle");
                });

            modelBuilder.Entity("TestTaskApi.Models.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NameAndSurname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TestTaskApi.Models.Entities.JobTitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("JobTitleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("JobTitles", t =>
                        {
                            t.HasCheckConstraint("CT_Restrict_value_in_boundaries", "`grade` >=1 AND `grade` <= 15");
                        });
                });

            modelBuilder.Entity("EmployeeJobTitle", b =>
                {
                    b.HasOne("TestTaskApi.Models.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestTaskApi.Models.Entities.JobTitle", null)
                        .WithMany()
                        .HasForeignKey("JobTitlesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
