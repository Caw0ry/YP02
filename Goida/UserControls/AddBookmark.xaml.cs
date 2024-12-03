using System.Windows;
using System.Windows.Controls;
using System;
using GoidaLibrary;
using System.IO;
namespace Goida.UserControls
{
    public partial class AddBookmark : UserControl
    {
        Bookmark bookmark = new Bookmark();
        public AddBookmark()
        {
            InitializeComponent();
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            ReturnToManagerMenu();
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssignBookmarkProperties();
                bookmark.IsFormValid();
                bookmark.IsArtValid(artx.txtInput.Text);
                bookmark.IsPriceDouble(pricex.txtInput.Text);
                bookmark.IsQuantityInt(quantityx.txtInput.Text);
                Bookmark actualBookmark = new Bookmark(bookmark.Isbn, bookmark.Name, bookmark.Edition, bookmark.Quantity, bookmark.Price);
                DataBase.AddItem(actualBookmark);
                DataBase.SaveItemInformation(actualBookmark);
                MessageBox.Show($"{actualBookmark.Name} has been successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearAllTextBoxes();
                ReturnToManagerMenu();
            }
            catch (ArgumentNullException ex)
            {
                ErrorMessage(ex);
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
            }
            catch (ItemAlreadyExistsException ex)
            {
                ErrorMessage(ex);
            }
            catch (IllegalIsbnException ex)
            {
                ErrorMessage(ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                DataBase.RemoveItem(artx.txtInput.Text);
            }
        }
        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ClearAllTextBoxes()
        {
            artx.txtInput.Clear();
            namex.txtInput.Clear();
            editionx.txtInput.Clear();
            quantityx.txtInput.Clear();
            pricex.txtInput.Clear();
        }
        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid AddBookmarkGrid = (Grid)mainWindow.FindName("AddBookmarkGrid");
            AddBookmarkGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            ClearAllTextBoxes();
        }
        private void AssignBookmarkProperties()
        {
            bookmark.Isbn = artx.txtInput.Text;
            bookmark.Name = namex.txtInput.Text;
            bookmark.Edition = editionx.txtInput.Text;
            bookmark.DateOfPrint = DateTime.Now;
        }
    }
}