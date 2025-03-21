using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstAPI.Data;
using MyFirstAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstAPI.Controllers
{
    [Authorize] // All endpoints require authentication
    [ApiController]
    [Route("api")]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;
        public BookController(BookContext context) => _context = context;

        [HttpGet("getbooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks() => await _context.Books.ToListAsync();

        [Authorize(Roles = "Admin")]
        [HttpPost("addbook")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updatebook/{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deletebook/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
