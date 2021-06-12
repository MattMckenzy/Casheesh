﻿// <auto-generated />
using System;
using Casheesh.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Casheesh.Migrations
{
    [DbContext(typeof(CasheeshContext))]
    partial class CasheeshContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Casheesh.Models.Account", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("CurrentBalance")
                        .HasColumnType("REAL");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.HasKey("Name");

                    b.HasIndex("Order");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Name = "Net Worth",
                            CurrentBalance = 0.0,
                            Order = 0
                        });
                });

            modelBuilder.Entity("Casheesh.Models.Balance", b =>
                {
                    b.Property<string>("AccountName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("AccountName", "Id");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("Casheesh.Models.Recurrence", b =>
                {
                    b.Property<string>("AccountName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastApplied")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TimeSpan")
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("AccountName", "Name");

                    b.ToTable("Recurrences");
                });

            modelBuilder.Entity("Casheesh.Models.Transaction", b =>
                {
                    b.Property<string>("AccountName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("AccountName", "Number");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Casheesh.Models.Balance", b =>
                {
                    b.HasOne("Casheesh.Models.Account", "Account")
                        .WithMany("Balances")
                        .HasForeignKey("AccountName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Casheesh.Models.Recurrence", b =>
                {
                    b.HasOne("Casheesh.Models.Account", "Account")
                        .WithMany("Recurrences")
                        .HasForeignKey("AccountName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Casheesh.Models.Transaction", b =>
                {
                    b.HasOne("Casheesh.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Casheesh.Models.Account", b =>
                {
                    b.Navigation("Balances");

                    b.Navigation("Recurrences");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
