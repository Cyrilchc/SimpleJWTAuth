using LibraryModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryData
{
    public class Context : DbContext
    {
        public DbSet<BookModel> Books { get; set; }
        public DbSet<AuthorModel> Authors { get; set; }
        private string DbPath { get; }
        
        public Context() => DbPath = "server=localhost;user=root;database=library;password=;";
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySql(DbPath, new MySqlServerVersion(new Version(5, 7, 36)));
    }
}