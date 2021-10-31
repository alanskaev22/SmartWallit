﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartWallit.Infrastructure.Data;

namespace SmartWallit.Infrastructure.Migrations
{
    [DbContext(typeof(WalletContext))]
    [Migration("20211031224252_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartWallit.Core.Entities.CardEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("CardNickname")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.Property<int>("Cvv")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("ExpirationMonth")
                        .HasMaxLength(2)
                        .HasColumnType("int");

                    b.Property<int>("ExpirationYear")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.WalletEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.CardEntity", b =>
                {
                    b.HasOne("SmartWallit.Core.Entities.WalletEntity", null)
                        .WithOne("Card")
                        .HasForeignKey("SmartWallit.Core.Entities.CardEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.WalletEntity", b =>
                {
                    b.HasOne("SmartWallit.Core.Entities.UserEntity", null)
                        .WithOne("Wallet")
                        .HasForeignKey("SmartWallit.Core.Entities.WalletEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.UserEntity", b =>
                {
                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("SmartWallit.Core.Entities.WalletEntity", b =>
                {
                    b.Navigation("Card");
                });
#pragma warning restore 612, 618
        }
    }
}
