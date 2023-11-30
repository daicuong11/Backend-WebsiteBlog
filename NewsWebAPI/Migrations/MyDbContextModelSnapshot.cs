﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsWebAPI.Data;

#nullable disable

namespace NewsWebAPI.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NewsWebAPI.Entities.Article", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleID"), 1L, 1);

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("View")
                        .HasColumnType("int");

                    b.HasKey("ArticleID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("UserID");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Content", b =>
                {
                    b.Property<int>("ContentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentID"), 1L, 1);

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.Property<string>("ContentBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContentIndex")
                        .HasColumnType("int");

                    b.Property<string>("ContentTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContentID");

                    b.HasIndex("ArticleID");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Image", b =>
                {
                    b.Property<int>("ImageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageID"), 1L, 1);

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.ImageArticleMapping", b =>
                {
                    b.Property<int>("ImageArticleMappingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageArticleMappingID"), 1L, 1);

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.Property<int>("ImageID")
                        .HasColumnType("int");

                    b.HasKey("ImageArticleMappingID");

                    b.HasIndex("ArticleID");

                    b.HasIndex("ImageID");

                    b.ToTable("ImageArticleMappings");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Love", b =>
                {
                    b.Property<int>("LoveID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoveID"), 1L, 1);

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.Property<int>("UserTargetID")
                        .HasColumnType("int");

                    b.HasKey("LoveID");

                    b.HasIndex("ArticleID");

                    b.ToTable("Loves");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"), 1L, 1);

                    b.Property<int>("ArticleTargetID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("UserTargetID")
                        .HasColumnType("int");

                    b.HasKey("NotificationID");

                    b.HasIndex("UserID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.SavedArticle", b =>
                {
                    b.Property<int>("SavedArticleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SavedArticleID"), 1L, 1);

                    b.Property<int>("ArticleID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SavedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserTargetID")
                        .HasColumnType("int");

                    b.HasKey("SavedArticleID");

                    b.HasIndex("ArticleID");

                    b.ToTable("SavedArticles");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Article", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsWebAPI.Entities.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Content", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.Article", "Article")
                        .WithMany("Contents")
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.ImageArticleMapping", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsWebAPI.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Love", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.Article", "Article")
                        .WithMany("Loves")
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Notification", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.User", "UserCreate")
                        .WithMany("Notifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserCreate");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.SavedArticle", b =>
                {
                    b.HasOne("NewsWebAPI.Entities.Article", "Article")
                        .WithMany("SavedArticles")
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.Article", b =>
                {
                    b.Navigation("Contents");

                    b.Navigation("Loves");

                    b.Navigation("SavedArticles");
                });

            modelBuilder.Entity("NewsWebAPI.Entities.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
