﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using stackblob.Infrastructure.Persistence;

#nullable disable

namespace stackblob.Infrastructure.Migrations
{
    [DbContext(typeof(StackblobDbContext))]
    [Migration("20240111173854_abc")]
    partial class abc
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("stackblob.Domain.Entities.Answer", b =>
                {
                    b.Property<string>("AnswerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("AnswerId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("QuestionId");

                    b.ToTable("ANSWER", (string)null);
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.Tag", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TagId");

                    b.ToTable("TAG", (string)null);
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("QuestionId");

                    b.HasIndex("CreatedById");

                    b.ToTable("QUESTION", (string)null);
                });

            modelBuilder.Entity("stackblob.Domain.Entities.QuestionTag", b =>
                {
                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("QuestionId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("QUESTION_TAG", (string)null);
                });

            modelBuilder.Entity("stackblob.Domain.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("USER", (string)null);
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Answer", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.User", "CreatedBy")
                        .WithMany("AnswersCreated")
                        .HasForeignKey("CreatedById");

                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.User", "CreatedBy")
                        .WithMany("QuestionsCreated")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.QuestionTag", b =>
                {
                    b.HasOne("stackblob.Domain.Entities.Question", "Question")
                        .WithMany("Tags")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stackblob.Domain.Entities.Lookup.Tag", "Tag")
                        .WithMany("Questions")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Lookup.Tag", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("stackblob.Domain.Entities.User", b =>
                {
                    b.Navigation("AnswersCreated");

                    b.Navigation("QuestionsCreated");
                });
#pragma warning restore 612, 618
        }
    }
}
