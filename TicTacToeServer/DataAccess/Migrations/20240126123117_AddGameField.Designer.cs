﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240126123117_AddGameField")]
    partial class AddGameField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GameField")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(9)
                        .HasColumnType("character(9)")
                        .HasDefaultValue("         ")
                        .IsFixedLength();

                    b.Property<long>("MaxRating")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("Player1Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Player2Id")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id")
                        .IsUnique();

                    b.HasIndex("Player2Id")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<int?>("AsWatcherId")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AsWatcherId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.HasOne("Domain.User", "Player1")
                        .WithOne("AsOwner")
                        .HasForeignKey("Domain.Game", "Player1Id");

                    b.HasOne("Domain.User", "Player2")
                        .WithOne("AsPlayer")
                        .HasForeignKey("Domain.Game", "Player2Id");

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.HasOne("Domain.Game", "AsWatcher")
                        .WithMany("Others")
                        .HasForeignKey("AsWatcherId");

                    b.Navigation("AsWatcher");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Navigation("Others");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("AsOwner");

                    b.Navigation("AsPlayer");
                });
#pragma warning restore 612, 618
        }
    }
}
