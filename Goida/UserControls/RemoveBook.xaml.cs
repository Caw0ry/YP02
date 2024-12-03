using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;

namespace Goida.UserControls
{
    public partial class RemoveBook : UserControl
    {
        Book book = new Book();

        public RemoveBook()
        {
            InitializeComponent();
            btnEnter.ButtonContent = Resex.btnEnter;
            btnreturn.ButtonContent = Resex.btnReturn;
            
            btnEnter.ButtonClickEvent += btnInput_Click;
            btnreturn.ButtonClickEvent += btnInput_Click;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e) 
        {
            if (sender == btnEnter)
            {
                try
                {
                    book = (Book)DataBase.FindItem(isbnBox.txtInput.Text);
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove the book: ({book.Name})?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataBase.RemoveItem(isbnBox.txtInput.Text);
                        DataBase.DeleteFile(book);
                        book = new Book();
                        MessageBox.Show("The book has been successfully removed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReturnToManagerMenu();
                    }
                }
                catch (IllegalIsbnException ex)
                {
                    ErrorMessage(ex);
                    isbnBox.txtInput.Focus();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    DataBase.AddItem(book);
                    isbnBox.txtInput.Focus();
                }
                catch (InvalidCastException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show("The ISBN you entered belongs to a journal!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    isbnBox.txtInput.Focus();
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
        Grid RemoveBookGrid = (Grid)mainWindow.FindName("RemoveBookGrid");
        RemoveBookGrid.Visibility = Visibility.Collapsed;

        Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
        managerGrid.Visibility = Visibility.Visible;
  
        isbnBox.txtInput.Text = string.Empty;
        }

    }
}
