using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;
using System.Threading.Tasks;
using DBAccessLibrary.DataAccess;
using DBAccessLibrary.Queries;
using JWTAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _db;

        public BooksController(IConfiguration config, ISqlDataAccess db)
        {
            _config = config;
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<BookModel>> Get()
        {
            // Vérifie si le jeton a été fourni par notre application
            TokenData tokenData = new TokenData(_db);
            bool isTokenDealtByMe =
                await tokenData.CheckTokenHasBeenGenerated(Request.Headers[HeaderNames.Authorization].ToString().Split(' ')[1]);
            if (!isTokenDealtByMe)
                StatusCode(498, "Le jeton n'est pas authentique");

            var resultBookList = new BookModel[]
            {
                new BookModel {Author = "Ray Bradbury", Title = "Fahrenheit 451"},
                new BookModel {Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude"},
                new BookModel {Author = "George Orwell", Title = "1984"},
                new BookModel {Author = "Anais Nin", Title = "Delta of Venus"}
            };
            
            #region Check age
            //var currentUser = HttpContext.User;
            //int userAge = 0;
            // if(currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            // {
            //     DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.DateOfBirth).Value);
            //     userAge = DateTime.Today.Year - birthDate.Year;
            // }
            //
            // if(userAge < 18){
            //     resultBookList = resultBookList.Where(x => !x.AgeRestriction).ToArray();
            // }
            #endregion
            
            return resultBookList;
        }
    }
}