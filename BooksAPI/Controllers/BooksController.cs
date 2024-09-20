using BooksAPI.Contacts;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Controllers
{
    [ApiController]   
    [Route("api/v1/books")]
   // [ApiVersion("1.0")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book bookDto)
        {
            var newBook = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                YearPublished = bookDto.YearPublished
            };

            var bookId = await _bookRepository.CreateBookAsync(newBook);
            newBook.Id = bookId;

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        // PUT: api/books/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book bookDto)
        {
            var updatedBook = new Book
            {
                Id = id,
                Title = bookDto.Title,
                Author = bookDto.Author,
                YearPublished = bookDto.YearPublished
            };

            var result = await _bookRepository.UpdateBookAsync(updatedBook);
            if (result == 0)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookRepository.DeleteBookAsync(id);
            if (result == 0)
                return NotFound();

            return NoContent();
        }
    }
}
