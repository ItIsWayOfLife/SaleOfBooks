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

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book()
                    {
                        GenreId = 1,
                        Code = "111",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 60,
                        Name = "Eating in the Age of Dieting",
                        Author = "Rujuta Diwekar",
                        Info = "Rujuta Diwekar is amongst the most followed nutritionists globally, and a leading health advocate. Over the past decade, her writings have decisively shifted food conversations across the country away from fads and towards eating local, seasonal and traditional. Her mantra, ‘eat local, think global’, blends the wisdom of our grandmothers with the latest advances in nutrition science for sustainable good health for all.",
                        Path = "EatingInTheAgeOfDieting.jpg",
                        PublishingHouse = "Print House",
                        YearOfWriting = "2020",
                        YearPublishing = "2020"
                    },
                    new Book()
                    {
                        GenreId = 2,
                        Code = "112",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 45,
                        Name = "Strength Training",
                        Author = "Rujuta Diwekar",
                        Info = "Weight training, more than any other form of exercise, is riddled with controversy, fear and suspicion. Rujuta Diwekar—India’s top health expert, and advisor to celebrities like Kareena Kapoor Khan, Alia Bhatt and Varun Dhawan, amongst others—demystifies strength training, and explains how you can make the most of your visit to the gym.",
                        Path = "StrengthTraining.jpg",
                        PublishingHouse = "State View",
                        YearOfWriting = "2018",
                        YearPublishing = "2019"
                    },
                    new Book()
                    {
                        GenreId = 1,
                        Code = "113",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 50,
                        Name = "Who Am I?",
                        Author = "Rujuta Diwekar",
                        Info = "Who am I? is the title given to a set of questions and answers bearing on Self-enquiry.Thisis the quintessential, aphoristic work, constituting one of the earliest recordings of the Maharshi s teachings,that spells out the nature of the Self and the practice of Self - Inquiry.",
                        Path = "WhoAmI.jpg",
                        PublishingHouse = "State View",
                        YearOfWriting = "2019",
                        YearPublishing = "2019"
                    },
                    new Book()
                    {
                        GenreId = 4,
                        Code = "114",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 65,
                        Name = "The Rock Babas And Other Stories",
                        Author = "Ameya Prabhu",
                        Info = "The Rock Babas and Other Stories is a map of human fragilities and resilience that is as true in the Himalayas as it is in the state of Georgia. An ageing tycoon, Takahashi Watanabe, donates his fortune to charity when faced with a terminal illness, in an attempt to reconnect with his estranged daughter. Special Agent Jackson Holder, an African-American agent of the Georgia Bureau of Investigation overcomes racial biases to crack a homicide with domestic hate crimes undertones. Swiss Hotelier Helmut Kauffman’s transforms under the tutelage of the eponymous ‘rock babas’, a group of monks playing rock and roll and living high up in the Himalayas. A deposed dictator in prison reflects on his life and times.",
                        Path = "TheRockBabasAndOtherStories.jpg",
                        PublishingHouse = "Print US",
                        YearOfWriting = "2018",
                        YearPublishing = "2018"
                    },
                    new Book()
                    {
                        GenreId = 4,
                        Code = "115",
                        IsDisplay = true,
                        IsFavorite = false,
                        IsNew = false,
                        Price = 63,
                        Name = "The Temple Tigers and More Man-Eaters of Kumaon",
                        Author = "Jim Corbett",
                        Info = "A continuation of the narratives in Man-eaters of Kumaon, The Temple Tiger and More Man-eaters of Kumaon further details Jim Corbett’s hunting exploits, as he is called upon to take down Tigers, Leopards and Bears in regions such as Dabidhura, Muktesar, Panar and Tanekpur. Apart from the hunts, the accounts vividly describe the flora, fauna, people and local legends of the areas Corbett went to and his experiences in them, ranging from hair-raising to rib-tickling.",
                        Path = "TheTempleTigersAndMoreMan-EatersOfKumaon.jpg",
                        PublishingHouse = "Print US",
                        YearOfWriting = "2020",
                        YearPublishing = "2020"
                    },
                    new Book()
                    {
                        GenreId = 5,
                        Code = "116",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 60,
                        Name = "Why Not Me? A Feeling of Millions ",
                        Author = "N. K. Singh",
                        Info = "N.K. Singh has been a formidable civil servant, an empathetic politician, a keen chronicler of India’s socioeconomic history and the quintessential academic that academia never got. His life’s work, as chronicled in this book has indeed been intertwined with the progress India has made. In many such cases, Singh has been not just an active contributor but has also given shape to those many momentous decisions—whether through the use of diplomacy or the rigours of understanding the mechanism of the levers of power or, for that matter, by consensus building.",
                        Path = "WhyNotMe-AFeelingOfMillions.jpg",
                        PublishingHouse = "State View",
                        YearOfWriting = "2020",
                        YearPublishing = "2020"
                    },
                    new Book()
                    {
                        GenreId = 6,
                        Code = "117",
                        IsDisplay = true,
                        IsFavorite = true,
                        IsNew = true,
                        Price = 60,
                        Name = "Gun Island (Hindi)",
                        Author = "Amitav Ghosh",
                        Info = "Bengali The Vinci Code… —The Sunday Times (London) Related to the two biggest issues of current time: climate change and human migration. The confidence with which Ghosh shapes a splendid story around these particular poles is awesome ... the way Ghosh is able to keep up with the pace of the novel, it is nothing short of a miracle ... Gun Island Is a novel of our time. - The Washington Post Shotgun A simple word, but this word turns the world of Din Dutta upside down.",
                        Path = "GunIsland(Hindi).jpg",
                        PublishingHouse = "Print US",
                        YearOfWriting = "2018",
                        YearPublishing = "2020"
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
