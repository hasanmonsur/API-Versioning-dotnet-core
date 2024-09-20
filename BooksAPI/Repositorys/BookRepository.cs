using Dapper;
using BooksAPI.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using BooksAPI.Contacts;
using BooksAPI.Data;

namespace BooksAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DapperContext _dapperContext;

        public BookRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        // Fetch all books
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            const string query = "SELECT * FROM Books";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<Book>(query);
            }
        }

        // Fetch book by ID
        public async Task<Book> GetBookByIdAsync(int id)
        {
            const string query = "SELECT * FROM Books WHERE Id = @Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Book>(query, new { Id = id });
            }
        }

        // Create a new book
        public async Task<int> CreateBookAsync(Book book)
        {
            const string query = "INSERT INTO Books (Title, Author, YearPublished) VALUES (@Title, @Author, @YearPublished);" +
                                 "SELECT CAST(SCOPE_IDENTITY() as int);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, book);
            }
        }

        // Update an existing book
        public async Task<int> UpdateBookAsync(Book book)
        {
            const string query = "UPDATE Books SET Title = @Title, Author = @Author, YearPublished = @YearPublished WHERE Id = @Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, book);
            }
        }

        // Delete a book by ID
        public async Task<int> DeleteBookAsync(int id)
        {
            const string query = "DELETE FROM Books WHERE Id = @Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
