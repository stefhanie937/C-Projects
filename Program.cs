using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class Book
{
    public int BookNumber { get; set; } // Unique identifier for each book
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; }
    public string Genre { get; set; } // New field for genre/classification
    public DateTime PublicationDate { get; set; } // New field for publication date
    public DateTime LastActionTime { get; set; }
    public string Description { get; set; } // New field for book description

    public Book(int bookNumber, string title, string author, bool isAvailable, string genre, DateTime publicationDate, string description)
    {
        BookNumber = bookNumber;
        Title = title;
        Author = author;
        IsAvailable = isAvailable;
        Genre = genre;
        PublicationDate = publicationDate;
        Description = description;
    }
}

public class Program
{
    private static List<Book> library = new List<Book>();
    private static int nextBookNumber = 1; 

    public static void Main(string[] args)
    {
        DisplayWelcomeMessage();  
    }

    public static void SaveBooksToFile(List<Book> books, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var book in books)
                {
                    writer.WriteLine($"{book.BookNumber}|{book.Title}|{book.Author}|{book.IsAvailable}|{book.Genre}|{book.PublicationDate:yyyy-MM-dd}|{book.Description}");
                }
            }
            Console.WriteLine("Library data has been backed up successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while backing up library: {ex.Message}");
        }
    }

    public static List<Book> LoadBooksFromFile(string filePath)
    {
        var books = new List<Book>();

        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        var book = new Book(
                            int.Parse(parts[0]),
                            parts[1],
                            parts[2],
                            bool.Parse(parts[3]),
                            parts[4],
                            DateTime.Parse(parts[5]),
                            parts[6]
                        );
                        books.Add(book);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while loading books from file: {ex.Message}");
        }

        return books;
    }


public static List<Book> SearchBooks(List<Book> books, string query)
{
    query = query.ToLower();
    return books.Where(b => b.Title.ToLower().Contains(query) || b.Author.ToLower().Contains(query) || b.Genre.ToLower().Contains(query)).ToList();
}

public static void DisplayBooks(List<Book> books, Func<Book, object> orderBy)
{
    var sortedBooks = books.OrderBy(orderBy).ToList();
    foreach (var book in sortedBooks)
    {
        Console.WriteLine($"{book.BookNumber}: {book.Title} by {book.Author} ({book.Genre}, {book.PublicationDate:yyyy}) - {(book.IsAvailable ? "Available" : "Not Available")}");
    }
}

public static void DisplayBookCount(List<Book> books)
{
    Console.WriteLine($"Total number of books: {books.Count}");
}

public static void BackupLibrary(List<Book> books, string backupFilePath)
{
    SaveBooksToFile(books, backupFilePath);
}

public static void RestoreLibrary(ref List<Book> books, string backupFilePath)
{
    books = LoadBooksFromFile(backupFilePath);
}

public static void ReportDamage(List<Book> books, int bookNumber)
{
    var book = books.FirstOrDefault(b => b.BookNumber == bookNumber);
    if (book != null)
    {
        book.IsAvailable = false;
        Console.WriteLine($"Book {book.Title} reported as damaged and marked as not available.");
    }
    else
    {
        Console.WriteLine("Book not found.");
    }
}


    public static void DisplayWelcomeMessage()
    {
        SetConsoleProperties();
        DisplayLogo();
        PromptUserToContinue();
    }

public static void SetConsoleProperties()
{
    
    Console.WindowHeight = 37;
    Console.WindowWidth = 69;
    Console.Title = "Library Management System";
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Magenta;
}

public static void DisplayLogo()
{
    Console.WriteLine("\n ▓▓  ▓▓  ▓▓ ▒▒▒▒▒▒▒ ░░       ▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒ ░░░░░░░░░░ ▓▓▓▓▓▓▓  ▒▒ ");
    Console.WriteLine(" ▓▓  ▓▓  ▓▓ ▒▒      ░░       ▓▓      ▒▒    ▒▒ ░░  ░░  ░░ ▓▓       ▒▒ ");
    Console.WriteLine(" ▓▓  ▓▓  ▓▓ ▒▒▒▒▒▒  ░░       ▓▓      ▒▒    ▒▒ ░░  ░░  ░░ ▓▓▓▓▓▓   ▒▒ ");
    Console.WriteLine(" ▓▓  ▓▓  ▓▓ ▒▒      ░░       ▓▓      ▒▒    ▒▒ ░░  ░░  ░░ ▓▓          ");
    Console.WriteLine(" ▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒ ░░░░░░░░ ▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒ ░░  ░░  ░░ ▓▓▓▓▓▓▓  ▒▒ ");
}

public static void PromptUserToContinue()
{
    Console.WriteLine("                      \n\n\nContinue to library?\n\n                 ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("                     ┌───────┐            ┌───────┐                  ");
    Console.WriteLine("                     │  YES  │            │   NO  │                  ");
    Console.WriteLine("                     └───────┘            └───────┘                  ");

    ConsoleKeyInfo key;
    do
    {
        key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.N)
        {
            Environment.Exit(0);
        }
        else if (key.Key != ConsoleKey.Y)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nInvalid input. Please press 'Y' to continue or 'N' to exit.");
        }
    } while (key.Key != ConsoleKey.Y);
    Console.ResetColor();

    DisplayFrontPage();
}

    public static void DisplayFrontPage()
    {
        DisplayHeader();
        DisplayPoem();
    }

    public static void DisplayHeader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌             Welcome to my Library Management System ♥           ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌  Info:                                                          ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌  Your gateway to organized and efficient library management.    ▐ ");
        Console.WriteLine(" ▌  With this system, you can effortlessly manage your collection. ▐ ");
        Console.WriteLine(" ▌  Browse, add, remove, borrow, and return books seamlessly,      ▐ ");
        Console.WriteLine(" ▌  empowering you to curate your library experience with ease.    ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▌                                                                 ▐ ");
        Console.WriteLine(" ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀\n\n ");
    }

    public static void DisplayPoem()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(" I have a short poem for you ☻");
        Console.WriteLine(" »»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»");
        Console.WriteLine("                Whispers of Wisdom by Aiyen Segovia\n                 ");
        Console.WriteLine("               In libraries vast, their spines align,                 ");
        Console.WriteLine("                 Echoes of ages, stories intertwine.                  ");
        Console.WriteLine("             With each turned page, a journey unfolds,                ");
        Console.WriteLine("                In the realm of books, magic behold.                  ");
        Console.WriteLine(" «««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««««\n\n");
        Console.ForegroundColor = ConsoleColor.Magenta;
        
        Console.WriteLine("\n\n\n\n\n\nPress any key to continue to main menu.");
        Console.ReadKey(true);
        DisplayMainMenu();
    }
    
public static void DisplayMainMenu()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine(" ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
    Console.WriteLine(" ▌                                                                  ▐ ");
    Console.WriteLine(" ▌            Welcome to our Library Management System ♥            ▐ ");
    Console.WriteLine(" ▌                                                                  ▐ ");
    Console.WriteLine(" ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ");
    Console.ResetColor();
    Console.WriteLine("\n MAIN MENU: \n\n\n");
    
    string[] menuItems = { "   ■ Display Books", "   ■ Add a Book", "   ■ Remove a Book", "   ■ Check Out a Book", "   ■ Check In a Book", "   ■ Backup Library", "   ■ Restore Library", "   ■ Exit" };
    int selectedOption = 0;
    while (true)
    {
        for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(menuItems[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(menuItems[i]);
                }
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = (selectedOption == 0) ? menuItems.Length - 1 : selectedOption - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = (selectedOption == menuItems.Length - 1) ? 0 : selectedOption + 1;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    HandleUserChoice(selectedOption + 1);
                    return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
            Console.WriteLine(" ▌                                                                  ▐ ");
            Console.WriteLine(" ▌            Welcome to our Library Management System ♥            ▐ ");
            Console.WriteLine(" ▌                                                                  ▐ ");
            Console.WriteLine(" ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ");
            Console.ResetColor();
            Console.WriteLine("\n MAIN MENU: \n\n\n");
        }
    }

public static void HandleUserChoice(int choice)
{
    switch (choice)
    {
        case 1:
            DisplayBooks();
            break;
        case 2:
            AddBook();
            break;
        case 3:
            RemoveBook();
            break;
        case 4:
            CheckOutBook();
            break;
        case 5:
            CheckInBook();
            break;
        case 6:
            BackupLibrary();
            break;
        case 7:
            RestoreLibrary();
            break;
        case 8:
            ExitApplication();
            break;
        default:
            break;
    }
}

    public static void GetInitialDetails(ConsoleColor color)
    {
        Console.Clear();
        Console.ForegroundColor = color;
        Console.WriteLine("Please supply the information requested below.\n");
        Console.ResetColor();
    }

    public static string GetBookInfo(string infoType)
    {
        string info;
        do
        {
            Console.Write($"   {infoType}: ");
            info = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(info))
            {
                Console.WriteLine($"{infoType} cannot be empty. Please try again.");
            }
        } while (string.IsNullOrWhiteSpace(info));
        return info;
    }

    public static void DisplayBooks()
{
    Console.Clear();
    if (library.Count == 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The library is currently empty.");
        Console.ResetColor();
    }
    else
    {
        var groupedBooks = library.GroupBy(book => book.Genre);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("║                             MY LIBRARY ☻                           ║");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        foreach (var group in groupedBooks)
        {
            Console.WriteLine($" Genre: {group.Key}\n");
            var sortedBooks = group.OrderBy(book => book.Title).ToList(); // Convert to list to access by index

            for (int i = 0; i < sortedBooks.Count; i++)
            {
                var book = sortedBooks[i];
                Console.WriteLine($"   {i + 1}. Book Number: {book.BookNumber} \n      Title: {book.Title} \n      Author: {book.Author} \n      Genre: {book.Genre} \n      Publication Date: {book.PublicationDate.ToShortDateString()} \n      Description: {book.Description} \n      Available: {(book.IsAvailable ? "Yes" : "No")} \n      Last Action Time: {book.LastActionTime}\n");
            }
        }
    }
    Console.WriteLine("\n\n\n\n\n\nPress any key to return to the main menu.");
    Console.ReadKey(true);
    DisplayMainMenu();
}

    public static void AddBook()
{
    do
    {
        GetInitialDetails(ConsoleColor.DarkGray);
        int bookNumber = GetUniqueBookNumber();
        string title = GetBookInfo("Title");
        string author = GetBookInfo("Author");
        string genre = GetBookInfo("Genre");
        DateTime publicationDate = GetPublicationDate();
        string description = GetBookInfo("Description");

        Book newBook = new Book
        {
            BookNumber = bookNumber,
            Title = title,
            Author = author,
            Genre = genre,
            PublicationDate = publicationDate,
            Description = description,
            IsAvailable = true,
            LastActionTime = DateTime.Now
        };
        library.Add(newBook);

        Console.WriteLine("\n\nBook has been successfully added to the library:\n");
        Console.WriteLine($"   Book Number: {newBook.BookNumber}\n   Title: {newBook.Title}\n   Author: {newBook.Author}\n   Genre: {newBook.Genre}\n   Publication Date: {newBook.PublicationDate.ToShortDateString()}\n   Description: {newBook.Description}\n   Available: Yes");

        Console.WriteLine("\n\n\n\n\n\nPress 'A' to add another book.");
        Console.WriteLine("Press another key to return to the main menu.");
    } 
    while (Console.ReadKey(true).Key == ConsoleKey.A);

    library = library.OrderBy(book => book.Genre).ThenBy(book => book.Title).ToList();
    DisplayMainMenu();
}

    public static int GetUniqueBookNumber()
{
    return library.Count > 0 ? library.Max(b => b.BookNumber) + 1 : 1;
}

    public static DateTime GetPublicationDate()
{
    DateTime publicationDate;
    while (true)
    {
        Console.Write("   Publication Date (yyyy-mm-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out publicationDate))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid date format. Please try again.");
        }
    }
    return publicationDate;
}

    public static void RemoveBook()
{
    while (true)
    {
        GetInitialDetails(ConsoleColor.DarkGray);
        int bookNumber = GetBookNumber();
        Book bookToRemove = library.Find(b => b.BookNumber == bookNumber);
        if (bookToRemove != null)
        {
            library.Remove(bookToRemove);
            Console.WriteLine("\n\nBook has been successfully removed from the library!");
        }
        else
        {
            Console.WriteLine("\n\n\nOops! It seems like the book you're searching for\nisn't in our collection.");
        }

        Console.WriteLine("\n\n\n\n\n\nPress 'A' to remove another book.");
        Console.WriteLine("Press another key to return to the main menu.");
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.A)
        {
            break;
        }
        DisplayMainMenu();
    }
}

    public static void CheckOutBook()
{
    while (true)
    {
        GetInitialDetails(ConsoleColor.DarkBlue);
        int bookNumber = GetBookNumber();
        Book bookToCheckOut = library.Find(b => b.BookNumber == bookNumber);
        if (bookToCheckOut != null)
        {
            if (bookToCheckOut.IsAvailable)
            {
                bookToCheckOut.IsAvailable = false;
                Console.WriteLine("\n\nBook has been successfully checked out!");
            }
            else
            {
                Console.WriteLine("\n\n\nOops! It seems like the book is already checked out.");
            }
        }
        else
        {
            Console.WriteLine("\n\n\nOops! It seems like the book you're searching for\nisn't in our collection.");
        }

        Console.WriteLine("\n\n\n\n\n\nPress 'C' to check out another book.");
        Console.WriteLine("Press another key to return to the main menu.");
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.C)
        {
            break;
        }
        DisplayMainMenu();
    }
}

    public static void CheckInBook()
    {
        while (true)
        {
            GetInitialDetails(ConsoleColor.DarkGreen);
            int bookNumber = GetBookNumber();
            Book bookToCheckIn = library.Find(b => b.BookNumber == bookNumber);
            if (bookToCheckIn != null)
            {
                if (!bookToCheckIn.IsAvailable)
                {
                    bookToCheckIn.IsAvailable = true;
                    Console.WriteLine("\n\nBook has been successfully checked in!");
                }
                else
                {
                    Console.WriteLine("\n\n\nOops! It seems like the book is already checked in.");
                }
            }
            else
            {
                Console.WriteLine("\n\n\nOops! It seems like the book you're searching for\nisn't in our collection.");
            }

            Console.WriteLine("\n\n\n\n\n\nPress 'C' to check in another book.");
            Console.WriteLine("Press another key to return to the main menu.");
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.C)
            {
                break;
            }
            DisplayMainMenu();
        }
    }


    public static int GetBookNumber()
    {
        int bookNumber;
        while (true)
        {
            Console.Write("   Book Number: ");
            if (int.TryParse(Console.ReadLine(), out bookNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid book number.");
            }
        }
        return bookNumber;
    }

    public static void PromptReturnToMainMenu()
    {
        Console.WriteLine("\n\n\n\n\n\nPress any key to return to the main menu.");
        Console.ReadKey(true);
        DisplayMainMenu();
    }

    public static void BackupLibrary()
{
    string filePath = "library_backup.txt";
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        foreach (Book book in library)
        {
            writer.WriteLine($"{book.BookNumber}|{book.Title}|{book.Author}|{book.Genre}|{book.PublicationDate.ToShortDateString()}|{book.Description}|{book.IsAvailable}|{book.LastActionTime}");
        }
    }
    Console.WriteLine("Library data has been backed up successfully.");
}

    public static void RestoreLibrary()
{
    string filePath = "library_backup.txt";
    if (File.Exists(filePath))
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            library.Clear();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('|');
                Book book = new Book
                {
                    BookNumber = int.Parse(parts[0]),
                    Title = parts[1],
                    Author = parts[2],
                    Genre = parts[3],
                    PublicationDate = DateTime.Parse(parts[4]),
                    Description = parts[5],
                    IsAvailable = bool.Parse(parts[6])
                };
                library.Add(book);
            }
        }
        Console.WriteLine("Library data has been restored successfully.");
    }
    else
    {
        Console.WriteLine("No backup file found.");
    }
}

    public static void ExitApplication()
    {
        Console.WriteLine("Are you sure you want to exit? (Y/N)");
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Y)
        {
            Environment.Exit(0);
        }
        else
        {
            DisplayMainMenu();
        }
    }
}


