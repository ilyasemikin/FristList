﻿// <auto-generated />
using System;
using FristList.Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FristList.Service.PublicApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220223211835_AddActivityAndCategoryFields")]
    partial class AddActivityAndCategoryFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BaseCategoryCurrentActivity", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrentActivitiesUserId")
                        .HasColumnType("uuid");

                    b.HasKey("CategoriesId", "CurrentActivitiesUserId");

                    b.HasIndex("CurrentActivitiesUserId");

                    b.ToTable("CurrentActivityCategories", (string)null);
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Account.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Account.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
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

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("BeginAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("EndAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.ActivityCategory", b =>
                {
                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("ActivityId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ActivityCategories", (string)null);
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.CurrentActivity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("BeginAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId");

                    b.ToTable("CurrentActivities");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Categories.Base.BaseCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseCategory");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Categories.PersonalCategory", b =>
                {
                    b.HasBaseType("FristList.Service.Data.Models.Categories.Base.BaseCategory");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasIndex("OwnerId");

                    b.HasDiscriminator().HasValue("PersonalCategory");
                });

            modelBuilder.Entity("BaseCategoryCurrentActivity", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Categories.Base.BaseCategory", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FristList.Service.Data.Models.Activities.CurrentActivity", null)
                        .WithMany()
                        .HasForeignKey("CurrentActivitiesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Account.RefreshToken", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Account.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.Activity", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Account.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.ActivityCategory", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Activities.Activity", "Activity")
                        .WithMany("Categories")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FristList.Service.Data.Models.Categories.Base.BaseCategory", "Category")
                        .WithMany("Activities")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.CurrentActivity", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Account.User", "User")
                        .WithOne()
                        .HasForeignKey("FristList.Service.Data.Models.Activities.CurrentActivity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Categories.PersonalCategory", b =>
                {
                    b.HasOne("FristList.Service.Data.Models.Account.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Activities.Activity", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("FristList.Service.Data.Models.Categories.Base.BaseCategory", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
