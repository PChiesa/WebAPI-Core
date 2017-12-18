﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebAPI.Database;
using WebAPI.Enums;

namespace WebAPI.Migrations
{
    [DbContext(typeof(WebApiContext))]
    partial class WebApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("WebAPI.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientEventId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description1");

                    b.Property<string>("Description2");

                    b.Property<string>("Description3");

                    b.Property<string>("Image1");

                    b.Property<string>("Image2");

                    b.Property<string>("Image3");

                    b.Property<string>("Name");

                    b.Property<int>("StoreId");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("WebAPI.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientStoreId");

                    b.Property<string>("Hash");

                    b.Property<string>("Name");

                    b.Property<string>("SecretKey");

                    b.HasKey("Id");

                    b.ToTable("Store");
                });

            modelBuilder.Entity("WebAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cpf");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebAPI.Models.Voucher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentStatus");

                    b.Property<string>("Description1");

                    b.Property<string>("Description2");

                    b.Property<string>("Description3");

                    b.Property<int>("EventId");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("Token");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Voucher");
                });

            modelBuilder.Entity("WebAPI.Models.Event", b =>
                {
                    b.HasOne("WebAPI.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
