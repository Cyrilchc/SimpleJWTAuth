using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryData;
using LibraryModels;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorModelsController : ControllerBase
    {
        private readonly Context _context;

        public AuthorModelsController(Context context)
        {
            _context = context;
        }

        // GET: api/AuthorModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorModel>>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        // GET: api/AuthorModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorModel>> GetAuthorModel(int id)
        {
            var authorModel = await _context.Authors.FindAsync(id);

            if (authorModel == null)
            {
                return NotFound();
            }

            return authorModel;
        }

        // PUT: api/AuthorModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthorModel(int id, AuthorModel authorModel)
        {
            if (id != authorModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(authorModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AuthorModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorModel>> PostAuthorModel(AuthorModel authorModel)
        {
            _context.Authors.Add(authorModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthorModel", new { id = authorModel.Id }, authorModel);
        }

        // DELETE: api/AuthorModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorModel(int id)
        {
            var authorModel = await _context.Authors.FindAsync(id);
            if (authorModel == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(authorModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorModelExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
