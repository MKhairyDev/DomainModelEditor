﻿// <auto-generated />
using System;
using DomainModelEditor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DomainModelEditor.Data.Migrations
{
    [DbContext(typeof(EntityContext))]
    partial class EntityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainModelEditor.Domain.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllowNull")
                        .HasColumnType("bit");

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("AttributeType")
                        .HasColumnType("int");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MaxValue")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MinValue")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Attributes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AllowNull = false,
                            AttributeName = "FirstName",
                            AttributeType = 1,
                            MaxValue = "50",
                            MinValue = "5"
                        });
                });

            modelBuilder.Entity("DomainModelEditor.Domain.Coord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EntityId")
                        .IsUnique();

                    b.ToTable("Coords");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EntityId = 1,
                            X = 100,
                            Y = 100
                        },
                        new
                        {
                            Id = 2,
                            EntityId = 2,
                            X = 200,
                            Y = 200
                        });
                });

            modelBuilder.Entity("DomainModelEditor.Domain.Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsPersistentEntity")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Entities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Car"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Wheels"
                        });
                });

            modelBuilder.Entity("DomainModelEditor.Domain.EntityAttributeValue", b =>
                {
                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId", "AttributeId");

                    b.HasIndex("AttributeId");

                    b.ToTable("EntityAttributeValue");
                });

            modelBuilder.Entity("DomainModelEditor.Domain.Coord", b =>
                {
                    b.HasOne("DomainModelEditor.Domain.Entity", null)
                        .WithOne("Coordination")
                        .HasForeignKey("DomainModelEditor.Domain.Coord", "EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DomainModelEditor.Domain.EntityAttributeValue", b =>
                {
                    b.HasOne("DomainModelEditor.Domain.Attribute", "Attribute")
                        .WithMany("Entities")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainModelEditor.Domain.Entity", "Entity")
                        .WithMany("Attributes")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
