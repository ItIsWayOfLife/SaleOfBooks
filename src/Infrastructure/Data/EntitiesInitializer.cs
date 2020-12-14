using Core.Entities;
using System.Linq;

namespace Infrastructure.Data
{
    public class EntitiesInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre() { Name = "Drama" },
                    new Genre() { Name = "Fantasy" },
                    new Genre() { Name = "Fairy tale" },
                    new Genre() { Name = "Travel books" },
                    new Genre() { Name = "Autobiography" },
                    new Genre() { Name = "Mystery" },
                    new Genre() { Name = "Comics" }
                );
                context.SaveChanges();
            }
        }
    }
}
