using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            int userAge = 0;
            var resultBookList = new Book[] {
                new Book { Author = "Ray Bradbury",Title = "Fahrenheit 451" },
                new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude" },
                new Book { Author = "George Orwell", Title = "1984" },
                new Book { Author = "Anais Nin", Title = "Delta of Venus" }
            };

            if(currentUser.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                DateTime birthDate = DateTime.Parse(currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.DateOfBirth).Value);
                userAge = DateTime.Today.Year - birthDate.Year;
            }

            if(userAge < 18){
                resultBookList = resultBookList.Where(x => !x.AgeRestriction).ToArray();
            }

            return resultBookList;
        }
    }
}