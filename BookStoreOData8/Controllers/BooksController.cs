using BookStoreOData8.DataSources;
using BookStoreOData8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace BookStoreOData8.Controllers
{
    public class BooksController : ODataController
    {
        private BookStoreContext _context;

        public BooksController(BookStoreContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (context.Books.Count() == 0)
            {
                var books = DataSource.GetBooks();
                foreach (var book in books)
                {
                    var existingPress = _context.Presses
                                                .FirstOrDefault(p => p.Name == book.Press.Name);
                    if (existingPress != null)
                    {
                        book.Press = existingPress;
                    }
                    else
                    {
                        _context.Presses.Add(book.Press);
                    }
                    _context.Books.Add(book);
                }       
                context.SaveChanges();
            }
        }

        // Method for 5 complex OData queries
        [EnableQuery]
        [HttpGet("odata/Books")]
        public IQueryable<Book> GetComplexBooks()
        {
            return _context.Books.Include(b => b.Press);
        }
    }
}
