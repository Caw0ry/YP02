using System.IO;
using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;
namespace Goida.UserControls
{
    public partial class Manager : UserControl
    {
        public Manager()
        {
            InitializeComponent();
            AssignNamesToButtons();
            AssignEventToButtons();
        }

        private void Button_Click(object sender, RoutedEventArgs e) 
        {
            Window mainWindow = Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            if (sender == addBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid addBookGrid = (Grid)mainWindow.FindName("addBookGrid");
                addBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == addJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid AddJournalGrid = (Grid)mainWindow.FindName("AddJournalGrid");
                AddJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == addBookmark)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid AddBookmarkGrid = (Grid)mainWindow.FindName("AddBookmarkGrid");
                AddBookmarkGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid editBookGrid = (Grid)mainWindow.FindName("editBookGrid");
                editBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid EditJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
                EditJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editBookmark)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid EditBookmarkGrid = (Grid)mainWindow.FindName("EditBookmarkGrid");
                EditBookmarkGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveBookGrid = (Grid)mainWindow.FindName("RemoveBookGrid");
                RemoveBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveJournalGrid = (Grid)mainWindow.FindName("RemoveJournalGrid");
                RemoveJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeBookmark)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveBookmarkGrid = (Grid)mainWindow.FindName("RemoveBookmarkGrid");
                RemoveBookmarkGrid.Visibility = Visibility.Visible;
            }
            else if (sender == showAvailableBooks)
            {
                int bookCount = DataBase.BookCount();
                if (bookCount == 0)
                {
                    MessageBox.Show("There are currently no books available for display!", "ERORR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowBooksGrid = (Grid)mainWindow.FindName("ShowBooksGrid");
                    ShowBooksGrid.Visibility = Visibility.Visible;

                    ShowBooks showBooksControl = (ShowBooks)ShowBooksGrid.Children[0];
                    showBooksControl.DisplayBooks();
                }
            }
            else if (sender == showAvailableJournals)
            {
                int journalCount = DataBase.JournalCount();
                if (journalCount == 0)
                {
                    MessageBox.Show("There are currently no journals available for display!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowJournalsGrid = (Grid)mainWindow.FindName("ShowJournalsGrid");
                    ShowJournalsGrid.Visibility = Visibility.Visible;

                    ShowJournal showJournalsControl = (ShowJournal)ShowJournalsGrid.Children[0];
                    showJournalsControl.DisplayJournals();
                }
            }
            else if (sender == showAvailableBookmarks)
            {
                int bookmarkCount = DataBase.BookmarkCount();
                if (bookmarkCount == 0)
                {
                    MessageBox.Show("There are currently no bookmarks available for display!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowBookmarksGrid = (Grid)mainWindow.FindName("ShowBookmarksGrid");
                    ShowBookmarksGrid.Visibility = Visibility.Visible;

                    ShowBookmarks showBookmarksControl = (ShowBookmarks)ShowBookmarksGrid.Children[0];
                    showBookmarksControl.DisplayBookmarks();
                }
            }
            else if (sender == showAvailableItems)
            {
                int itemCount = DataBase.ItemCount();
                if (itemCount == 0)
                {
                    MessageBox.Show("There are currently no items available for display!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowAvailableItemsGrid = (Grid)mainWindow.FindName("ShowAvailableItemsGrid");
                    ShowAvailableItemsGrid.Visibility = Visibility.Visible;

                    ShowAvailableItems ShowAvailableItemsControl = (ShowAvailableItems)ShowAvailableItemsGrid.Children[0];
                    ShowAvailableItemsControl.DisplayItems();
                }
            }
            else if (sender == managePurchases)
            {
                try
                {
                    DataBase.DoesReceiptsDirExist();
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ReceiptsGrid = (Grid)mainWindow.FindName("ReceiptsGrid");
                    ReceiptsGrid.Visibility = Visibility.Visible;
                    Receipts ReceiptsControl = (Receipts)ReceiptsGrid.Children[0];
                    ReceiptsControl.DisplayReceipts();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (sender == manageException)
            {
                try
                {
                    DataBase.DoesExceptionsDirExist();
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ManageExceptionGrid = (Grid)mainWindow.FindName("ManageExceptionGrid");
                    ManageExceptionGrid.Visibility = Visibility.Visible;
                    ManageException ManageExceptionsControl = (ManageException)ManageExceptionGrid.Children[0];
                    ManageExceptionsControl.DisplayExceptions();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (sender == clubCustomers)
            {
                int customerCount = Customer.GetCustomersCount();
                if (customerCount == 0)
            {
                MessageBox.Show("There are currently no customers in the club!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid ShowClubCustomersGrid = (Grid)mainWindow.FindName("ShowClubCustomersGrid");
                ShowClubCustomersGrid.Visibility = Visibility.Visible;
                ShowClubCustomers ShowClubCustomersControl = (ShowClubCustomers)ShowClubCustomersGrid.Children[0];
                ShowClubCustomersControl.DisplayCustomersIds();
            }
        }
        else if (sender == mainmenu)
        {
            managerGrid.Visibility = Visibility.Collapsed;
            Grid mainMenu = (Grid)mainWindow.FindName("MainMenu");
            mainMenu.Visibility = Visibility.Visible;
        }
        else
        {
            Application.Current.Shutdown();
        }
    }
        private void AssignEventToButtons() 
        {
            addBook.ButtonClickEvent += Button_Click;
            addJournal.ButtonClickEvent += Button_Click;
            addBookmark.ButtonClickEvent += Button_Click;
            editBook.ButtonClickEvent += Button_Click;
            editJournal.ButtonClickEvent += Button_Click;
            editBookmark.ButtonClickEvent += Button_Click;
            removeBook.ButtonClickEvent += Button_Click;
            removeJournal.ButtonClickEvent += Button_Click;
            removeBookmark.ButtonClickEvent += Button_Click;
            showAvailableBooks.ButtonClickEvent += Button_Click;
            showAvailableJournals.ButtonClickEvent += Button_Click;
            showAvailableBookmarks.ButtonClickEvent += Button_Click;
            showAvailableItems.ButtonClickEvent += Button_Click;
            managePurchases.ButtonClickEvent += Button_Click;
            manageException.ButtonClickEvent += Button_Click;
            clubCustomers.ButtonClickEvent += Button_Click;
            mainmenu.ButtonClickEvent += Button_Click;
            exit.ButtonClickEvent += Button_Click;
        }

        private void AssignNamesToButtons() 
        {
            addBook.ButtonContent = Resex.btnAddBook;
            addJournal.ButtonContent = Resex.btnAddJournal;
            addBookmark.ButtonContent = Resex.btnAddBookmark;
            editBook.ButtonContent = Resex.btnEditBook;
            editJournal.ButtonContent = Resex.btnEditJournal;
            editBookmark.ButtonContent = Resex.btnEditBookmark;
            removeBook.ButtonContent = Resex.btnRemoveBook;
            removeJournal.ButtonContent = Resex.btnRemoveJournal;
            removeBookmark.ButtonContent = Resex.btnRemoveBookmark;
            showAvailableBooks.ButtonContent = Resex.btnShowBooks;
            showAvailableJournals.ButtonContent = Resex.btnShowJournals;
            showAvailableBookmarks.ButtonContent = Resex.btnShowBookmarks;
            showAvailableItems.ButtonContent = Resex.btnShowItems;
            managePurchases.ButtonContent = Resex.btnPurcchases;
            manageException.ButtonContent = Resex.btnManageExceptions;
            clubCustomers.ButtonContent = Resex.btnClubCustomers;
            mainmenu.ButtonContent = Resex.btnMainMenu;
            exit.ButtonContent = Resex.btnExit;
        }
    }
}


        

