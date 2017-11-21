using System;
using System.Globalization;
using System.Linq;
using System.Text;
using BookShop.Models;

namespace BookShop
{
    using BookShop.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                // 1. Age Restriction
                //string command = Console.ReadLine().ToLower();
                //StringBuilder bookTitles = new StringBuilder();
                //bookTitles.Append(GetBooksByAgeRestriction(db, command));
                //Console.WriteLine(bookTitles.ToString());

                //2.Golden Books
                //string goldenBooks = GetGoldenBooks(db);
                //Console.WriteLine(goldenBooks);

                // 3. Books by Price
                //string booksByPrice = GetBooksByPrice(db);
                //Console.WriteLine(booksByPrice);

                // 4. Not Released In
                //int year = int.Parse(Console.ReadLine());
                //string booksRelease = GetBooksNotRealeasedIn(db, year);
                //Console.WriteLine(booksRelease);

                // 5. Book Titles by Category
                //string input = Console.ReadLine().Trim();
                //string booksByCategory = GetBooksByCategory(db, input);
                //Console.WriteLine(booksByCategory);

                // 6. Released Before Date
                //string input = Console.ReadLine();
                //string booksReleasedBeforeDate = GetBooksReleasedBefore(db, input);
                //Console.WriteLine(booksReleasedBeforeDate);

                // 7. Author Search
                //string input = Console.ReadLine();
                //string booksAuthors = GetAuthorNamesEndingIn(db, input);
                //Console.WriteLine(booksAuthors);

                // 8. Book Search
                //string input = Console.ReadLine();
                //string booksTitles = GetBookTitlesContaining(db, input);
                //Console.WriteLine(booksTitles);

                // 9. Book Search by Author
                //string input = Console.ReadLine();
                //string booksTitleAuthor = GetBooksByAuthor(db, input);
                //Console.WriteLine(booksTitleAuthor);

                // 10. Count Books
                //int len = int.Parse(Console.ReadLine());
                //int count = CountBooks(db, len);
                //Console.WriteLine(count);

                // 11. Total Book Copies
                //string result = CountCopiesByAuthor(db);
                //Console.WriteLine(result);

                // 12.	Profit by Category
                //string profitByCategory = GetTotalProfitByCategory(db);
                //Console.WriteLine(profitByCategory);

                // 13. Most Recent Books
                //string mostRecentBooks = GetMostRecentBooks(db);
                //Console.WriteLine(mostRecentBooks);

                // 14. Increase Prices
                //IncreasePrices(db);

                // 15. Remove Books
                //int rowsAffected = RemoveBooks(db);
                //Console.WriteLine($"{rowsAffected} books were deleted");
            }
        }


        public static int RemoveBooks(BookShopContext db)
        {
            var book = db.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int rowsAffected = book.Count;

            db.Books.RemoveRange(book);
            db.SaveChanges();

            return rowsAffected;
        } // 15

        public static void IncreasePrices(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            db.SaveChanges();
        } // 14

        public static string GetMostRecentBooks(BookShopContext db)
        {
            var books = db.Categories
                .OrderBy(c => c.CategoryBooks.Select(cb => cb.BookId).Count())
                .ThenBy(c => c.Name)

                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(b => new
                        {
                            Title = b.Book.Title,
                            ReleaseDate = b.Book.ReleaseDate.Value
                        })
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .Select(b => $"{b.Title} ({b.ReleaseDate.Year})")
                        .ToList()
                })
                .Select(c => $"--{c.CategoryName}{Environment.NewLine}{string.Join(Environment.NewLine, c.Books)}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 13

        public static string GetTotalProfitByCategory(BookShopContext db)
        {
            var books = db.Categories
                .Select(c => new
                {
                    c.Name,
                    Sum = c.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum()
                })
                .OrderByDescending(c => c.Sum)
                .ThenBy(c => c.Name)
                .Select(c => $"{c.Name} ${c.Sum:f2}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 12

        public static string CountCopiesByAuthor(BookShopContext db)
        {
            var sum = db.Books
                .Select(b => new
                {
                    Name = $"{b.Author.FirstName} {b.Author.LastName}",
                    Sum = b.Author.Books.Select(a => a.Copies).Sum()
                })
                .OrderByDescending(b => b.Sum)
                .Select(b => $"{b.Name} - {b.Sum}")
                .Distinct()
                .ToList();

            return string.Join(Environment.NewLine, sum);
        } // 11

        public static int CountBooks(BookShopContext db, int len)
        {
            var count = db.Books
                .Select(b => b.Title)
                .Count(b => b.Length > len);

            return count;
        } // 10

        public static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    b.Author.FirstName,
                    b.Author.LastName
                })
                .Where(b => b.LastName.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                .Select(b => $"{b.Title} ({b.FirstName} {b.LastName})")
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 9

        public static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var books = db.Books
                .Select(b => b.Title)
                .Where(b => b.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 8

        public static string GetAuthorNamesEndingIn(BookShopContext db, string input)
        {
            var books = db.Books
                .Select(b => new
                {
                    b.Author.FirstName,
                    b.Author.LastName
                })
                .Where(b => b.FirstName.EndsWith(input))
                .Distinct()
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.FirstName} {b.LastName}").OrderBy(b => b).ToList());
        } // 7

        public static string GetBooksReleasedBefore(BookShopContext db, string input)
        {
            var date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = db.Books
                .Where(b => b.ReleaseDate.Value < date)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 6

        public static string GetBooksByCategory(BookShopContext db, string input)
        {
            var categories = input.Split(" \t\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var books = db.BookCategory
                .Where(b => categories.Contains(b.Category.Name, StringComparer.InvariantCultureIgnoreCase))

                .Select(b => b.Book.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 5

        public static string GetBooksNotRealeasedIn(BookShopContext db, int year)
        {
            var books = db.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        } // 4

        public static string GetBooksByPrice(BookShopContext db)
        {
            var book = db.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToList();

            return string.Join(Environment.NewLine, book);
        } // 3

        public static string GetGoldenBooks(BookShopContext db)
        {
            var books = db.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();


            return string.Join(Environment.NewLine, books);
        } // 2

        public static string GetBooksByAgeRestriction(BookShopContext db, string command)
        {
            int eNum = -1;

            if (command.Equals(AgeRestriction.Adult.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                eNum = (int)AgeRestriction.Adult;
            }

            if (command.Equals(AgeRestriction.Minor.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                eNum = (int)AgeRestriction.Minor;
            }

            if (command.Equals(AgeRestriction.Teen.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                eNum = (int)AgeRestriction.Teen;
            }

            var bookTitles = db.Books
                .Where(b => (int)b.AgeRestriction == eNum)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendJoin(Environment.NewLine, bookTitles);

            return sb.ToString();
        } // 1


    }
}
