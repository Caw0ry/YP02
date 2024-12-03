using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;
namespace Goida.UserControls
{
    public partial class RemoveBookmark : UserControl 
    {
        Bookmark bookmark = new Bookmark();
        public RemoveBookmark()
        {
            InitializeComponent();
            btnEnter.ButtonContent = Resex.btnEnter;
            btnReturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnReturn.ButtonClickEvent += btnInput_Click;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e) 
        {
            if (sender == btnEnter)
            {
                try
                {
                    bookmark = (Bookmark)DataBase.FindItem(artBox.txtInput.Text);
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove the bookmark: ({bookmark.Name})?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataBase.RemoveItem(artBox.txtInput.Text);
                        DataBase.DeleteFile(bookmark);
                        bookmark = new Bookmark();
                        MessageBox.Show("The bookmark has been successfully removed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReturnToManagerMenu();
                    }
                }
                catch (IllegalArtException ex)
                {
                    ErrorMessage(ex);
                    artBox.txtInput.Focus();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    DataBase.AddItem(bookmark);
                    artBox.txtInput.Focus();
                }
                catch (InvalidCastException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show("The ART you entered belongs to a book!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    artBox.txtInput.Focus();
                }
            }
            else
            {
                ReturnToManagerMenu();
            }
        }
        
        private void ErrorMessage(Exception ex) 
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private void ReturnToManagerMenu() 
        {
            Window mainWindow = Window.GetWindow(this);
            Grid RemoveBookmarkGrid = (Grid)mainWindow.FindName("RemoveBookmarkGrid");
            RemoveBookmarkGrid.Visibility = Visibility.Collapsed;

            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            artBox.txtInput.Text = string.Empty;
        }

    }
}
