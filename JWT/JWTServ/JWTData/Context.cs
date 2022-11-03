using JWTModels;
using Microsoft.EntityFrameworkCore;

namespace JWTData
{
    public class Context : DbContext
    {
        public DbSet<TokenModel> AccessTokens { get; set; }
        public DbSet<UserModel> Users { get; set; }

        private string DbPath { get; }

        public Context()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "jwtdata.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
        
        //public Context() => DbPath = "server=localhost;user=root;database=jwt;password=;";

        // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySql(DbPath, new MySqlServerVersion(new Version(5, 7, 36)));
    }
}