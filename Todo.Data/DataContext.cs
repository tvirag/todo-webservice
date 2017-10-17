using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Todo.Interfaces;
using Todo.Model;

namespace Todo.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext()
        {
          
           
        }
        public DbSet<TodoItem> TodoItems { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           
            //this.Database.EnsureDeleted();
            this.Database.EnsureCreated(); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(b => b.Name)
                .IsRequired();
        }

        public void Save()
        {
           base.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            //base.SaveChanges();
            base.Dispose();
        }
    }
}
