﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using stackblob.Infrastructure.Persistence;

#nullable disable

namespace stackblob.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(StackblobDbContext))]
    [Migration("20230402103206_Init Mig from Backup")]
    partial class InitMigfromBackup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("QuestionTag", b =>
                {
                    b.Property<int>("QuestionsQuestionId")
                        .HasColumnType("int");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.HasKey("QuestionsQuestionId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("QuestionTag");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerId"), 1L, 1);

                    b.Property<int?>("CorrectAnswerQuestionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("AnswerId");

                    b.HasIndex("CorrectAnswerQuestionId")
                        .IsUnique()
                        .HasFilter("[CorrectAnswerQuestionId] IS NOT NULL");

                    b.HasIndex("CreatedById");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Attachment", b =>
                {
                    b.Property<int>("AttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttachmentId"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserBannerPictureId")
                        .HasColumnType("int");

                    b.Property<int?>("UserProfilePictureId")
                        .HasColumnType("int");

                    b.HasKey("AttachmentId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TypeId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedByInAnswerId")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedByInQuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CommentId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("CreatedByInAnswerId");

                    b.HasIndex("CreatedByInQuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.LoginLocation", b =>
                {
                    b.Property<int>("LoginLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginLocationId"), 1L, 1);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TimeZoneId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginLocationId");

                    b.HasIndex("CountryId");

                    b.HasIndex("TimeZoneId");

                    b.HasIndex("UserId");

                    b.ToTable("LoginLocations");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.AttachmentTypeType", b =>
                {
                    b.Property<int>("AttachmentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AttachmentTypeId");

                    b.ToTable("AttachmentTypes");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"), 1L, 1);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.RoleTypeType", b =>
                {
                    b.Property<int>("RoleTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleTypeId");

                    b.ToTable("RoleTypes");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.SocialTypeType", b =>
                {
                    b.Property<int>("SocialTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("SocialTypeId");

                    b.ToTable("SocialTypes");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.TimeZone", b =>
                {
                    b.Property<int>("TimeZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimeZoneId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<TimeSpan>("Offset")
                        .HasColumnType("time");

                    b.HasKey("TimeZoneId");

                    b.ToTable("TimeZones");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"), 1L, 1);

                    b.Property<int?>("CorrectAnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionIdAccess")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("QuestionId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("QuestionIdAccess")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<string>("Token")
                        .HasMaxLength(64)
                        .HasColumnType("nchar(64)")
                        .IsFixedLength();

                    b.Property<bool>("AlreadyUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Token");

                    b.HasIndex("UserID");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int?>("BannerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nchar(64)")
                        .IsFixedLength();

                    b.Property<int?>("ProfilePictureId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("StatusText")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.HasIndex("BannerId")
                        .IsUnique()
                        .HasFilter("[BannerId] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("ProfilePictureId")
                        .IsUnique()
                        .HasFilter("[ProfilePictureId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.UserEmailVerification", b =>
                {
                    b.Property<int>("UserEmailVerificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserEmailVerificationId"), 1L, 1);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserEmailVerificationAccess")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserEmailVerificationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEmailVerifications");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.UserSocialType", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("SocialTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId", "SocialTypeId");

                    b.HasIndex("SocialTypeId");

                    b.ToTable("UserSocialTypes");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoteId"), 1L, 1);

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int?>("CreateByInAnswerId")
                        .HasColumnType("int");

                    b.Property<int?>("CreateByInQuestionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUpVote")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("VoteId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("CreateByInAnswerId");

                    b.HasIndex("CreateByInQuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("QuestionTag", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stackblob.Domain.Entities.Lookup.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Answer", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Question", "CorrectAnswerQuestion")
                        .WithOne("CorrectAnswer")
                        .HasForeignKey("stackblob.Domain.Entities.Answer", "CorrectAnswerQuestionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("stackblob.Domain.Entities.User", "CreatedBy")
                        .WithMany("Answers")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CorrectAnswerQuestion");

                    b.Navigation("CreatedBy");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Attachment", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Answer", "Answer")
                        .WithMany("Attachments")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("Attachments")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.Lookup.AttachmentTypeType", "Type")
                        .WithMany("Attachments")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Comment", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Answer", "Answer")
                        .WithMany("Comments")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("stackblob.Domain.Entities.User", "CreatedByInAnswer")
                        .WithMany("AnswerComments")
                        .HasForeignKey("CreatedByInAnswerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.User", "CreatedByInQuestion")
                        .WithMany("QuestionComments")
                        .HasForeignKey("CreatedByInQuestionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("Comments")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("CreatedByInAnswer");

                    b.Navigation("CreatedByInQuestion");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.LoginLocation", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Lookup.Country", "Country")
                        .WithMany("LoginLocations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("stackblob.Domain.Entities.Lookup.TimeZone", "TimeZone")
                        .WithMany("LoginLocations")
                        .HasForeignKey("TimeZoneId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("stackblob.Domain.Entities.User", "User")
                        .WithMany("LoginLocations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("stackblob.Domain.ValueObjects.IpAddress", "IpAddress", b1 =>
                        {
                            b1.Property<int>("LoginLocationId")
                                .HasColumnType("int");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(36)
                                .HasColumnType("nvarchar(36)")
                                .HasColumnName("IpAddress");

                            b1.HasKey("LoginLocationId");

                            b1.ToTable("LoginLocations");

                            b1.WithOwner()
                                .HasForeignKey("LoginLocationId");
                        });

                    b.Navigation("Country");

                    b.Navigation("IpAddress")
                        .IsRequired();

                    b.Navigation("TimeZone");

                    b.Navigation("User");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.User", "CreatedBy")
                        .WithMany("QuestionsCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.User", null)
                        .WithMany("Question")
                        .HasForeignKey("UserId");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.User", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Attachment", "Banner")
                        .WithOne("UserBannerPicture")
                        .HasForeignKey("stackblob.Domain.Entities.User", "BannerId");

                    b.HasOne("stackblob.Domain.Entities.Attachment", "ProfilePicture")
                        .WithOne("UserProfilePicture")
                        .HasForeignKey("stackblob.Domain.Entities.User", "ProfilePictureId");

                    b.Navigation("Banner");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.UserEmailVerification", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.User", "User")
                        .WithMany("EmailVerficiations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.UserSocialType", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Lookup.SocialTypeType", "SocialType")
                        .WithMany("Users")
                        .HasForeignKey("SocialTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stackblob.Domain.Entities.User", "User")
                        .WithMany("Socials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SocialType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Vote", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Answer", "Answer")
                        .WithMany("AnswerVotes")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.User", "CreateByInAnswer")
                        .WithMany("AnswerVotes")
                        .HasForeignKey("CreateByInAnswerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.User", "CreateByInQuestion")
                        .WithMany("QuestionVotes")
                        .HasForeignKey("CreateByInQuestionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("QuestionVotes")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("CreateByInAnswer");

                    b.Navigation("CreateByInQuestion");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Answer", b =>
                {
                    b.Navigation("AnswerVotes");

                    b.Navigation("Attachments");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Attachment", b =>
                {
                    b.Navigation("UserBannerPicture");

                    b.Navigation("UserProfilePicture");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.AttachmentTypeType", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.Country", b =>
                {
                    b.Navigation("LoginLocations");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.SocialTypeType", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.TimeZone", b =>
                {
                    b.Navigation("LoginLocations");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Attachments");

                    b.Navigation("Comments");

                    b.Navigation("CorrectAnswer");

                    b.Navigation("QuestionVotes");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.User", b =>
                {
                    b.Navigation("AnswerComments");

                    b.Navigation("AnswerVotes");

                    b.Navigation("Answers");

                    b.Navigation("EmailVerficiations");

                    b.Navigation("LoginLocations");

                    b.Navigation("Question");

                    b.Navigation("QuestionComments");

                    b.Navigation("QuestionVotes");

                    b.Navigation("QuestionsCreated");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Socials");
                });
#pragma warning restore 612, 618
        }
    }
}