﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDo.Models;

#nullable disable

namespace ToDo.Migrations
{
    [DbContext(typeof(ToDoDbContext))]
    [Migration("20230308040022_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ToDo.Models.Division", b =>
                {
                    b.Property<Guid>("DivisionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DivisionId");

                    b.ToTable("t_Division", (string)null);
                });

            modelBuilder.Entity("ToDo.Models.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DivisionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("JobTitleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("JobTitleId");

                    b.ToTable("t_Employee", (string)null);
                });

            modelBuilder.Entity("ToDo.Models.JobTitle", b =>
                {
                    b.Property<Guid>("JobTitleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("JobTitleId");

                    b.ToTable("t_JobTitle", (string)null);
                });

            modelBuilder.Entity("ToDo.Models.ToDoItem", b =>
                {
                    b.Property<Guid>("TodoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<Guid>("InsertEmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InsertTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Orders")
                        .HasColumnType("int");

                    b.Property<Guid>("UpdateEmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("TodoId");

                    b.HasIndex("InsertEmployeeId");

                    b.HasIndex("UpdateEmployeeId");

                    b.ToTable("t_TodoList", (string)null);
                });

            modelBuilder.Entity("ToDo.Models.Employee", b =>
                {
                    b.HasOne("ToDo.Models.Division", "Division")
                        .WithMany("Employees")
                        .HasForeignKey("DivisionId")
                        .IsRequired()
                        .HasConstraintName("FK_Employee_DivisionId");

                    b.HasOne("ToDo.Models.JobTitle", "JobTitle")
                        .WithMany("Employees")
                        .HasForeignKey("JobTitleId")
                        .IsRequired()
                        .HasConstraintName("FK_Employee_JobTitleId");

                    b.Navigation("Division");

                    b.Navigation("JobTitle");
                });

            modelBuilder.Entity("ToDo.Models.ToDoItem", b =>
                {
                    b.HasOne("ToDo.Models.Employee", "InsertEmployee")
                        .WithMany("TodoListInsertEmployees")
                        .HasForeignKey("InsertEmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_Todo_InsertEmpId");

                    b.HasOne("ToDo.Models.Employee", "UpdateEmployee")
                        .WithMany("TodoListUpdateEmployees")
                        .HasForeignKey("UpdateEmployeeId")
                        .IsRequired()
                        .HasConstraintName("FK_Todo_UpdateEmpId");

                    b.Navigation("InsertEmployee");

                    b.Navigation("UpdateEmployee");
                });

            modelBuilder.Entity("ToDo.Models.Division", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("ToDo.Models.Employee", b =>
                {
                    b.Navigation("TodoListInsertEmployees");

                    b.Navigation("TodoListUpdateEmployees");
                });

            modelBuilder.Entity("ToDo.Models.JobTitle", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
