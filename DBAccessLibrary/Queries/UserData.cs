using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using DBAccessLibrary.DataAccess;
using DBAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace DBAccessLibrary.Queries
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfigurationRoot _configurationRoot;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
            _configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json")
                .Build();
        }
        
        /// <summary>
        /// Récupère l'utilisateur correspondant au mail et au mot de passe passé en paramètres
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DataUserModel> GetUserByMail(string mail)
        {
            string sql = $"select * from user where email='{mail}';";
            List<DataUserModel> rawUsers = await _db.LoadData<DataUserModel, dynamic>(sql, new { });
            if (rawUsers.Count > 1)
                throw new Exception("Plusieurs utilisateurs correspondent à cette description");

            return rawUsers.Any() ? rawUsers.First() : null;
        }

        /// <summary>
        /// Vérifie si un utilisateur en base utilise le mail en paramètre
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public async Task<bool> CheckMailExists(string mail)
        {
            string sql = $"select * from user where email='{mail}';";
            return (await _db.LoadData<DataUserModel, dynamic>(sql, new { })).Count > 0;
        }

        /// <summary>
        /// Insère un nouvel utilisateur
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public Task InserUser(DataUserModel userModel)
        {
            string sql = @"insert into user (givenname, familyname, email, password, salt) values (@GivenName, @FamilyName, @Email, @Password, @Salt)";
            return _db.SaveData(sql, userModel);
        }
    }
}