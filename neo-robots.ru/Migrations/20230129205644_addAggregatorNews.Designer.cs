﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SMI.Data.Entities;

namespace SMI.Migrations
{
    [DbContext(typeof(SmiContext))]
    [Migration("20230129205644_addAggregatorNews")]
    partial class addAggregatorNews
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SMI.Data.Entities.AggregatorNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AggregatorSourceId")
                        .HasColumnType("int");

                    b.Property<string>("Html")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SourceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AggregatorSourceId");

                    b.ToTable("AggregatorNews");
                });

            modelBuilder.Entity("SMI.Data.Entities.AggregatorSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RssUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AggregatorSources");
                });

            modelBuilder.Entity("SMI.Data.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("SMI.Data.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.ToTable("HashTags");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTagsNews", b =>
                {
                    b.Property<int>("HashTagId")
                        .HasColumnType("int");

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.HasKey("HashTagId", "NewsId");

                    b.HasIndex("NewsId");

                    b.ToTable("HashTagsNews");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTagsTheme", b =>
                {
                    b.Property<int>("HashTagId")
                        .HasColumnType("int");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("HashTagId", "ThemeId");

                    b.HasIndex("ThemeId");

                    b.ToTable("HashTagsThemes");
                });

            modelBuilder.Entity("SMI.Data.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AggregatorNewsId")
                        .HasColumnType("int");

                    b.Property<int?>("AggregatorSourceId")
                        .HasColumnType("int");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Intro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublish")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NewspapersId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("PhotoId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasMaxLength(4000)
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("AggregatorNewsId")
                        .IsUnique()
                        .HasFilter("[AggregatorNewsId] IS NOT NULL");

                    b.HasIndex("AggregatorSourceId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("NewspapersId");

                    b.HasIndex("Path")
                        .IsUnique()
                        .HasFilter("[Path] IS NOT NULL");

                    b.HasIndex("PhotoId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsCities", b =>
                {
                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.HasKey("NewsId", "CityId");

                    b.HasIndex("CityId");

                    b.ToTable("NewsCities");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsRegion", b =>
                {
                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("NewsId", "RegionId");

                    b.HasIndex("RegionId");

                    b.ToTable("NewsRegions");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsTheme", b =>
                {
                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("NewsId", "ThemeId");

                    b.HasIndex("ThemeId");

                    b.ToTable("NewsThemes");
                });

            modelBuilder.Entity("SMI.Data.Entities.Newspaper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Newspapers");
                });

            modelBuilder.Entity("SMI.Data.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("SMI.Data.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("SMI.Data.Entities.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Sorting")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SMI.Data.Entities.AggregatorNews", b =>
                {
                    b.HasOne("SMI.Data.Entities.AggregatorSource", "AggregatorSource")
                        .WithMany()
                        .HasForeignKey("AggregatorSourceId")
                        .HasConstraintName("FK_AggregatorNews_AggregatorSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AggregatorSource");
                });

            modelBuilder.Entity("SMI.Data.Entities.City", b =>
                {
                    b.HasOne("SMI.Data.Entities.Region", "Region")
                        .WithMany("Cities")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTagsNews", b =>
                {
                    b.HasOne("SMI.Data.Entities.HashTag", "HashTag")
                        .WithMany("HashTagsNews")
                        .HasForeignKey("HashTagId")
                        .HasConstraintName("FK_HashTagsNews_HashTags")
                        .IsRequired();

                    b.HasOne("SMI.Data.Entities.News", "News")
                        .WithMany("HashTagsNews")
                        .HasForeignKey("NewsId")
                        .HasConstraintName("FK_HashTagsNews_News")
                        .IsRequired();

                    b.Navigation("HashTag");

                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTagsTheme", b =>
                {
                    b.HasOne("SMI.Data.Entities.HashTag", "HashTag")
                        .WithMany("HashTagsThemes")
                        .HasForeignKey("HashTagId")
                        .HasConstraintName("FK_HashTagsThemes_HashTags")
                        .IsRequired();

                    b.HasOne("SMI.Data.Entities.Theme", "Theme")
                        .WithMany("HashTagsThemes")
                        .HasForeignKey("ThemeId")
                        .HasConstraintName("FK_HashTagsThemes_Themes")
                        .IsRequired();

                    b.Navigation("HashTag");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("SMI.Data.Entities.News", b =>
                {
                    b.HasOne("SMI.Data.Entities.AggregatorNews", "AggregatorNews")
                        .WithOne("News")
                        .HasForeignKey("SMI.Data.Entities.News", "AggregatorNewsId")
                        .HasConstraintName("FK_AggregatorNews_AggregatorNews");

                    b.HasOne("SMI.Data.Entities.AggregatorSource", "AggregatorSource")
                        .WithMany("News")
                        .HasForeignKey("AggregatorSourceId")
                        .HasConstraintName("FK_News_AggregatorSources");

                    b.HasOne("SMI.Data.Entities.Author", "Author")
                        .WithMany("News")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_News_Authors");

                    b.HasOne("SMI.Data.Entities.Newspaper", "Newspapers")
                        .WithMany("News")
                        .HasForeignKey("NewspapersId")
                        .HasConstraintName("FK_News_Newspapers");

                    b.HasOne("SMI.Data.Entities.Photo", "Photo")
                        .WithMany("News")
                        .HasForeignKey("PhotoId")
                        .HasConstraintName("FK_News_Photos");

                    b.Navigation("AggregatorNews");

                    b.Navigation("AggregatorSource");

                    b.Navigation("Author");

                    b.Navigation("Newspapers");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsCities", b =>
                {
                    b.HasOne("SMI.Data.Entities.City", "Cities")
                        .WithMany("NewsCities")
                        .HasForeignKey("CityId")
                        .HasConstraintName("FK_NewsCities_Cities")
                        .IsRequired();

                    b.HasOne("SMI.Data.Entities.News", "News")
                        .WithMany("NewsCities")
                        .HasForeignKey("NewsId")
                        .HasConstraintName("FK_NewsCities_News")
                        .IsRequired();

                    b.Navigation("Cities");

                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsRegion", b =>
                {
                    b.HasOne("SMI.Data.Entities.News", "News")
                        .WithMany("NewsRegions")
                        .HasForeignKey("NewsId")
                        .HasConstraintName("FK_NewsRegions_News")
                        .IsRequired();

                    b.HasOne("SMI.Data.Entities.Region", "Region")
                        .WithMany("NewsRegions")
                        .HasForeignKey("RegionId")
                        .HasConstraintName("FK_NewsRegions_Regions")
                        .IsRequired();

                    b.Navigation("News");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("SMI.Data.Entities.NewsTheme", b =>
                {
                    b.HasOne("SMI.Data.Entities.News", "News")
                        .WithMany("NewsThemes")
                        .HasForeignKey("NewsId")
                        .HasConstraintName("FK_NewsThemes_News")
                        .IsRequired();

                    b.HasOne("SMI.Data.Entities.Theme", "Theme")
                        .WithMany("NewsThemes")
                        .HasForeignKey("ThemeId")
                        .HasConstraintName("FK_NewsThemes_Themes")
                        .IsRequired();

                    b.Navigation("News");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("SMI.Data.Entities.AggregatorNews", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.AggregatorSource", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.Author", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.City", b =>
                {
                    b.Navigation("NewsCities");
                });

            modelBuilder.Entity("SMI.Data.Entities.HashTag", b =>
                {
                    b.Navigation("HashTagsNews");

                    b.Navigation("HashTagsThemes");
                });

            modelBuilder.Entity("SMI.Data.Entities.News", b =>
                {
                    b.Navigation("HashTagsNews");

                    b.Navigation("NewsCities");

                    b.Navigation("NewsRegions");

                    b.Navigation("NewsThemes");
                });

            modelBuilder.Entity("SMI.Data.Entities.Newspaper", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.Photo", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("SMI.Data.Entities.Region", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("NewsRegions");
                });

            modelBuilder.Entity("SMI.Data.Entities.Theme", b =>
                {
                    b.Navigation("HashTagsThemes");

                    b.Navigation("NewsThemes");
                });
#pragma warning restore 612, 618
        }
    }
}
