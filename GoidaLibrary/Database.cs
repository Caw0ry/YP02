using System.Text;
using System.Text.Json;

namespace GoidaLibrary
{
    public static class DataBase
    {
        static List<AbstractItem> libraryItems = new List<AbstractItem>();
        static string dataBase = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase";

        public static void SaveItemInformation(AbstractItem item)
        {
            CheckForDbExistence();
            string bookDirectory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Books";
            if (!Directory.Exists(bookDirectory)) throw new DirectoryNotFoundException("Book Database directory could not be found!");
            string journalDirectory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Journals";
            if (!Directory.Exists(journalDirectory)) throw new DirectoryNotFoundException("Journal data base directory could not be found!");
            string bookmarkDirectory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Bookmarks";
            if (!Directory.Exists(bookmarkDirectory)) throw new DirectoryNotFoundException("Bookmark data base directory could not be found!");

            string directory = "";
            string fileName = "";

            if (item.GetType() == typeof(Book))
            {
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Books\Book" + " " + item.Isbn;
                fileName = "BookInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Book)item);
                File.WriteAllText(filePath, itemSerialized);
            }
            else if (item.GetType() == typeof(Journal))
            {
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Journals\Journal" + " " + item.Isbn;
                fileName = "JournalInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Journal)item);
                File.WriteAllText(filePath, itemSerialized);
            }
            else
            {
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Bookmarks\Bookmark" + " " + item.Isbn;
                fileName = "BookmarkInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Bookmark)item);
                File.WriteAllText(filePath, itemSerialized);
            }
        }

        public static void LoadItems() //загрузка информации о товарах из базы данных
        {
            CheckForDbExistence();

            string directoryPathBooks = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Books";
            string directoryPathJournals = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Journals";
            string directoryPathBookmarks = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Bookmarks";

            if (Directory.Exists(directoryPathBooks))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathBooks, "BookInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Book book = JsonSerializer.Deserialize<Book>(txtString); // пpeoбpaзование информации из JSON þaйла
                    AddItem(book);
                }
            }
            else throw new DirectoryNotFoundException("Books directory could not be found!");

            if (Directory.Exists(directoryPathJournals))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathJournals, "JournalInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Journal journal = JsonSerializer.Deserialize<Journal>(txtString);
                    AddItem(journal);
                }
            }
            else throw new DirectoryNotFoundException("Journals directory could not be found!");

            if (Directory.Exists(directoryPathBookmarks))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathBookmarks, "BookmarkInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Bookmark bookmark = JsonSerializer.Deserialize<Bookmark>(txtString);
                    AddItem(bookmark);
                }
            }
            else throw new DirectoryNotFoundException("Bookmark directory could not be found!");
        }

        public static void DeleteFile(AbstractItem item) //удаление файла с информацией о товаре 
        {
            string directory = "";
            if (item.GetType() == typeof(Book))
            {
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Books\Book" + " " + item.Isbn;
            }
            else if (item.GetType() == typeof(Journal))
            {
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Journals\Journal" + " " + item.Isbn;
            }
            else
            {
                Bookmark bookmark = new Bookmark();
                directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Bookmarks\Bookmark" + " " + bookmark.Isbn;
            }
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
            else throw new DirectoryNotFoundException("The directory could not be found!");
        }

        public static void SaveReceipt(List<AbstractItem> items, double totalPrice, bool ClubDiscount = false) //сoxранение чека
        {
            CheckForDbExistence();

            string itemReceiptsDirectory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\ItemReceipts";
            if (!Directory.Exists(itemReceiptsDirectory)) throw new DirectoryNotFoundException("Receipts directory could not be found!");

            string directory = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\ItemReceipts\Receipt" + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string fileName = $"Receipt.txt";

            Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, fileName);

            StringBuilder receipt = new StringBuilder(); 
            receipt.AppendLine(DateTime.Now.ToString());
            receipt.AppendLine("Items Bought:");

            foreach (var item in items)
            {
                string itemInfo = item.PartialToString();
                receipt.AppendLine(itemInfo);
            }
            if (ClubDiscount == false) receipt.AppendLine($"Total Price: {totalPrice:C}");
            else receipt.AppendLine($"Total Price After Club Discount: {totalPrice:C}");

            File.WriteAllText(filePath, receipt.ToString());
        }

        public static List<string> GetReceipts() //метод получения чека с информацией о покупке
        {
            CheckForDbExistence();

            var receipts = new List<string>();
            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\ItemReceipts";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "Receipt.txt", SearchOption.AllDirectories))
                {
                    string receipt = File.ReadAllText(filePath);
                    receipts.Add(receipt);
                }
                return receipts;
            }
            else throw new DirectoryNotFoundException("Receipts directory could not be found!");
        }

        public static List<string> GetExceptions() //метод получения возникающих исключений
        {
            CheckForDbExistence();

            var exceptions = new List<string>();
            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Exceptions";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "exception.txt", SearchOption.AllDirectories))
                {
                    string exception = File.ReadAllText(filePath); exceptions.Add(exception);
                    exceptions.Add(exception);
                }
                return exceptions;
            }
            else throw new DirectoryNotFoundException("Exceptions directory could not be found!");
        }

        public static List<string> GetCustomers() //метод получения списка покупателей
        {
            CheckForDbExistence();

            var customers = new List<string>();
            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Customers";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "CustomerId.txt", SearchOption.AllDirectories))
                {
                    string customerId = File.ReadAllText(filePath);
                    customers.Add(customerId);
                }
                return customers;
            }
            else throw new DirectoryNotFoundException("Customers directory could not be found!");
        }

        public static AbstractItem GetItemByIndex(int index) //мeтoд получения товара по индексу
        {
            return libraryItems[index];
        }
        public static void LogException(Exception exception)
        {
            if (!Directory.Exists(dataBase)) return;

            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Exceptions\Exception" + exception.GetType().Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string fileName = "exception.txt";
            string directoryPathExceptions = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Exceptions";
            if (Directory.Exists(directoryPathExceptions))
            {
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, fileName);
                string exceptionText = $"Exception: {exception} \nDate: {DateTime.Now}";
                File.WriteAllText(filePath, exceptionText);
            }
            else return;
        }
        public static void DoesReceiptsDirExist() //метод проверки существвания папки с чеками
        {
            CheckForDbExistence();

            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\ItemReceipts";
            if (Directory.Exists(directoryPath))
            {
                int length = Directory.GetDirectories(directoryPath).Length;
                if (length == 0)
                {
                    throw new DirectoryNotFoundException("There are currently no receipts available for display");
                }
            } 
            else throw new DirectoryNotFoundException("Receipts directory could not be found!");
        }

        public static void DoesExceptionsDirExist() //метод проверки существвания папки с исключениями 
        {
            CheckForDbExistence();

            string directoryPath = @"C:\Users\vissa\OneDrive\Desktop\practica2\GoidaLibrary\GoidaBase\Exceptions";
            if (Directory.Exists(directoryPath))
            {
                int length = Directory.GetDirectories(directoryPath).Length;
                if (length == 0)
                {
                    throw new DirectoryNotFoundException("There are currently no Exceptions available for display");
                }
            }
            else throw new DirectoryNotFoundException("Exceptions directory could not be found!");
        }

        public static void CheckForDbExistence() //метод проверки существования базы данных
        {
            if (!Directory.Exists(dataBase)) throw new DirectoryNotFoundException("Data base directory could not be found!");
        }

        public static List<AbstractItem> FilterItemsByName(String name) //мeToд выборки товара по наименованио
        {
            var itemsFoundWithName = new List<AbstractItem>();
            foreach (var item in libraryItems)
            {
                if (HasContinuousSubstring(item.Name, name)) itemsFoundWithName.Add(item);
            }
            if (itemsFoundWithName.Count == 0) throw new ArgumentException("The name you entered does not relate to any of the items in the library!");
            return itemsFoundWithName;
        }

        public static bool HasContinuousSubstring(string itemName, string name) //проверка на соответствие размера имени с искомым именем 
        {
            int nameIndex = 0;
            int counter = 0;
            for (int i = 0; i < itemName.Length; i++)
            {
                if (counter == name.Length) return true;
                if (name[nameIndex] == itemName[i])
                {
                    counter++;
                    nameIndex++;
                }
                else
                {
                    counter = 0;
                    nameIndex = 0;
                }
            }
            return counter == name.Length;
        }

        public static List<AbstractItem> GetItems() //получение товаров
        {
            return libraryItems;
        }
        public static List<AbstractItem> GetBooks() //получение книг
        {
            return libraryItems.Where(item => item is Book).ToList();
        }
        public static List<AbstractItem> GetJournals() //получение журналов
        {
            return libraryItems.Where(item => item is Journal).ToList();
        }
        public static List<AbstractItem> GetBookmarks() 
        {
            return libraryItems.Where(item => item is Bookmark).ToList();
        }
        public static int ItemCount() //получение количества товаров
        {
            return libraryItems.Count;
        }
        public static int BookCount() //получение количества книг
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Book)) count++;
            }
            return count;
        }

        public static int JournalCount() //получение количества журналов
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Journal)) count++;
            }
            return count;
        }
        public static int BookmarkCount()
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Bookmark)) count++;
            }
            return count;
        }

        public static void AddItem(AbstractItem item) //метод добавления товара в базу
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == item.Isbn)
                {
                    throw new ItemAlreadyExistsException("This item already exists in the library!");
                }
            }
            libraryItems.Add(item);
        }

        public static void IsIsbnAvailable(string isbn) //метод проверки доступности кода товара
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    throw new IllegalIsbnException("This ISBN already exists in the library!");
                }
            }
        }
        public static void IsIsbnValid(string isbn) //метод валидации кода товара
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    return;
                }
            }
            throw new IllegalIsbnException("The ISBN you entered does not match an existing item!");
        }
        public static void IsArtAvailable(string isbn) 
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    throw new IllegalArtException("This ART already exists in the library!");
                }
            }
        }
        public static void IsArtValid(string isbn) 
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    return;
                }
            }
            throw new IllegalArtException("The ART you entered does not match an existing item!");
        }
        public static AbstractItem FindItem(string isbn) //метод нажождения товара
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    return existingItem;
                }
            }
            throw new IllegalIsbnException("The ISBN you entered does not match an existing item!");
        }


        public static void RemoveItem(string isbn) //мeтод удаления товара
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    libraryItems.Remove(existingItem);
                    return;
                }
            }
            throw new IllegalIsbnException("The ISBN you entered does not match an existing item!");
        }
    }
}

