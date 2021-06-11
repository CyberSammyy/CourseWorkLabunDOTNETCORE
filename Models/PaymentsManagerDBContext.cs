using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL_DOTNETCORE.Models
{
    public static class PathToDbCreator
    {
        /// <summary>
        /// Input here a path to the database file. Do it in a proper way!
        /// Example:
        /// C:\Users\kaila\source\repos\DAL_DOTNETCORE\Payments.mdf
        /// </summary>
        /// <param name="path"></param>
        public static void PathCreate(string path)
        {
            PaymentsManagerDBContext.PathToTheDB = path;
        }
    }

    internal class PaymentsManagerDBContext : DbContext
    {
        public static string PathToTheDB { get; set; } = @"D:\Payments.mdf";
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={PathToTheDB};Integrated Security=True");
        }
    }
}

