﻿using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Casheesh.Models
{
    public class CasheeshContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Recurrence> Recurrences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(account => account.Order);

            modelBuilder.Entity<Account>()
                .HasData(new Account { Name = "Net Worth" });

            modelBuilder.Entity<Balance>()
                .HasKey(balance => new { balance.AccountName, balance.Id });
   
            modelBuilder.Entity<Transaction>()
                .HasKey(transaction => new { transaction.AccountName, transaction.Number });

            modelBuilder.Entity<Recurrence>()
                .HasKey(recurrence => new { recurrence.AccountName, recurrence.Name });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Copy database to folder if it doesn't exist
            FileInfo databaseSourceFileInfo = new("casheesh.db");
            Directory.CreateDirectory("/data");
            FileInfo databaseDestinationFileInfo = new("/data/casheesh.db");

            if (!databaseDestinationFileInfo.Exists)
                databaseSourceFileInfo.CopyTo(databaseDestinationFileInfo.FullName);

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source=\"{databaseDestinationFileInfo.FullName}\";");
        }
    }
}