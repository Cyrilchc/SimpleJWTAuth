using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DBAccessLibrary.DataAccess;
using DBAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace DBAccessLibrary.Queries
{
    public class TokenData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot _configurationRoot;

        public TokenData(ISqlDataAccess db)
        {
            _db = db;
            _configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// Insert en base un token généré par l'application
        /// </summary>
        /// <param name="dataGeneratedTokenModel"></param>
        /// <returns></returns>
        public Task InsertToken(DataGeneratedTokenModel dataGeneratedTokenModel)
        {
            string sql = @"insert into generatedtoken (token) values (@Token)";
            return _db.SaveData(sql, dataGeneratedTokenModel);
        }
            
        /// <summary>
        /// Vérifie que le token en paramètre existe dans la base de données
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CheckTokenHasBeenGenerated(string token)
        {
            string sql = $"select * from generatedtoken where token ='{token}';";
            return (await _db.LoadData<DataGeneratedTokenModel, dynamic>(sql, new { })).Count > 0;
        }
    }
}