﻿// <auto-generated />
using DemoApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoApplication.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250211053522_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.Property<int>("CoursescourseId")
                        .HasColumnType("int");

                    b.Property<int>("stdId")
                        .HasColumnType("int");

                    b.HasKey("CoursescourseId", "stdId");

                    b.HasIndex("stdId");

                    b.ToTable("CourseStudent");
                });

            modelBuilder.Entity("DemoApplication.Model.Entity.Course", b =>
                {
                    b.Property<int>("courseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("courseId"));

                    b.Property<string>("courseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("teacherId")
                        .HasColumnType("int");

                    b.HasKey("courseId");

                    b.HasIndex("teacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("DemoApplication.Model.Entity.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("studentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("DemoApplication.Model.Entity.Teacher", b =>
                {
                    b.Property<int>("teacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("teacherId"));

                    b.Property<string>("teacherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("teacherId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.HasOne("DemoApplication.Model.Entity.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursescourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemoApplication.Model.Entity.Student", null)
                        .WithMany()
                        .HasForeignKey("stdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoApplication.Model.Entity.Course", b =>
                {
                    b.HasOne("DemoApplication.Model.Entity.Teacher", "teacher")
                        .WithMany("Courses")
                        .HasForeignKey("teacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("teacher");
                });

            modelBuilder.Entity("DemoApplication.Model.Entity.Teacher", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
