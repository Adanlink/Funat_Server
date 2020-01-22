﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Database.Context;

namespace Server.Database.Migrations
{
    [DbContext(typeof(FunatDbContext))]
    [Migration("20200101172148_TryFixForRelation")]
    partial class TryFixForRelation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Server.Database.Models.AccountModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(254)")
                        .HasMaxLength(254);

                    b.Property<string>("EncodedHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PreferedLanguage")
                        .HasColumnType("int");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SessionIdExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeOfCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Server.Database.Models.CharacterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Authority")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("LastTimePlayed")
                        .HasColumnType("datetime2");

                    b.Property<long>("MapId")
                        .HasColumnType("bigint");

                    b.Property<float>("MapX")
                        .HasColumnType("real");

                    b.Property<float>("MapY")
                        .HasColumnType("real");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)")
                        .HasMaxLength(24);

                    b.Property<long?>("OwnerAccountId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeOfCreation")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Server.Database.Models.CharacterModel", b =>
                {
                    b.HasOne("Server.Database.Models.AccountModel", "OwnerAccount")
                        .WithMany("Characters")
                        .HasForeignKey("OwnerAccountId");
                });
#pragma warning restore 612, 618
        }
    }
}
