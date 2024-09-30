using BookStoreOData8.Models;
using Bogus;
using System.Collections.Generic;

namespace BookStoreOData8.DataSources
{
    public static class DataSource
    {
        private static IList<Book> listBooks { get; set; }
        private static IList<Press> listPresses { get; set; }

        public static IList<Book> GetBooks()
        {
            if (listBooks != null)
            {
                return listBooks;
            }

            listBooks = new List<Book>();
            listPresses = new List<Press>();

            var pressFaker = new Faker<Press>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Company.CompanyName())
                .RuleFor(p => p.Category, f => f.PickRandom<Category>());

            listPresses = pressFaker.Generate(10);

            var bookFaker = new Faker<Book>()
                .RuleFor(b => b.Id, f => f.IndexFaker + 1)
                .RuleFor(b => b.ISBN, f => f.Commerce.Ean13())
                .RuleFor(b => b.Title, f => f.Commerce.ProductName())
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Price, f => f.Finance.Amount(10, 100)) 
                .RuleFor(b => b.Location, f => new Address
                {
                    City = f.Address.City(),
                    Street = f.Address.StreetAddress()
                })
                .RuleFor(b => b.Press, f => f.PickRandom(listPresses)); 

            listBooks = bookFaker.Generate(100);

            return listBooks;
        }

        public static IList<Press> GetPresses()
        {
            if (listPresses != null)
            {
                return listPresses;
            }

            var pressFaker = new Faker<Press>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Company.CompanyName())
                .RuleFor(p => p.Category, f => f.PickRandom<Category>());

            listPresses = pressFaker.Generate(10);

            return listPresses;
        }
    }
}
