﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Screen_Capture.MVVM.Model;

namespace Screen_Capture.MVVM.Model
{
    [DbContext(typeof(HotKeyDBContext))]
    [Migration("20210805073234_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17");

            modelBuilder.Entity("Screen_Capture.MVVM.Model.HotKeys", b =>
                {
                    b.Property<long>("HotKeyId")
                        .HasColumnName("HotKeyID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Alt")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Button")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Ctrl")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Shift")
                        .HasColumnType("INTEGER");

                    b.HasKey("HotKeyId");

                    b.HasIndex("HotKeyId")
                        .IsUnique();

                    b.ToTable("HotKeys");
                });
#pragma warning restore 612, 618
        }
    }
}