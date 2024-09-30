using BookStoreOData8.DataSources;
using BookStoreOData8.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace BookStoreOData8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddOData(options => options.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count().AddRouteComponents("odata", GetEdmModel()));

            // Register BookStoreContext with In-Memory Database
            builder.Services.AddDbContext<BookStoreContext>(options =>
                options.UseInMemoryDatabase("BookStoreDb"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Seed the in-memory database with data
            SeedDatabase(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            // Define OData entities and sets
            odataBuilder.EntitySet<Book>("Books");
            odataBuilder.EntitySet<Press>("Presses");
            return odataBuilder.GetEdmModel();
        }

        private static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreContext>();

                // Add books from the DataSource class
                var books = DataSource.GetBooks();

                dbContext.Books.AddRange(books);
                dbContext.SaveChanges();
            }
        }
    }
}
