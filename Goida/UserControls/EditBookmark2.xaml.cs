using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using GoidaLibrary;
using System.IO;
using Microsoft.Office.Interop.Word;
using Bookmark = GoidaLibrary.Bookmark;
using Window = System.Windows.Window;
namespace Goida.UserControls
{
    public partial class EditBookmark2 : UserControl
    {
        Bookmark bookmark = new Bookmark();
        public EditBookmark2()
        {
            InitializeComponent();

            btnEnter.ButtonContent = Resex.btnEnter;
            btnReturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnReturn.ButtonClickEvent += btnInput_Click;
        }

        public void PopulateComboBox()
        {
            if (comboBox.Items.Count == 0)
            {
                comboBox.Items.Add($"ART ({bookmark.Isbn})");
                comboBox.Items.Add($"Name ({bookmark.Name})");
                comboBox.Items.Add($"Edition ({bookmark.Edition})");
                comboBox.Items.Add($"Quantity ({bookmark.Quantity})");
                comboBox.Items.Add($"Price ({bookmark.Price})");
            }
        }

        public void ClearComboBox()
        {
            comboBox.Items.Clear();
        }

        public void FindBookmark()
        {
            try
            {
                bookmark = new Bookmark();
                bookmark = (Bookmark)DataBase.FindItem(EditBookmark.Art!);
            }
            catch (InvalidCastException ex)
            {
                DataBase.LogException(ex);
                ReturnToFirstMenu();
                MessageBox.Show("The ART you entered to a book!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnEnter)
            {
                if (comboBox.SelectedItem != null)
                {
                    ProceedToChangeProperty();
                }
                else
                {
                    MessageBox.Show("Please choose an option!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ReturnToFirstMenu();
            }
        }
        private void btnEnter2_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                EditArt();
            }
            else if (comboBox.SelectedIndex == 1)
            {
                EditName();
            }
            else if (comboBox.SelectedIndex == 2)
            {
                EditEdition();
            }
            else if (comboBox.SelectedIndex == 3)
            {
                EditQuantity();
            }
            else
            {
                EditPrice();
            }
        }

        private void btnReturn2_Click(object sender, RoutedEventArgs e)
        {
            ReturnToOptionSelectionMenu();
        }

        private void ProceedToChangeProperty()
        {
            btnEnter.Visibility = Visibility.Collapsed;
            btnReturn.Visibility = Visibility.Collapsed;
            viewBoxCombo.Visibility = Visibility.Collapsed;

            viewBoxEnter2.Visibility = Visibility.Visible;
            viewBoxReturn2.Visibility = Visibility.Visible;
            viewBoxTextBox.Visibility = Visibility.Visible;

            string boundText = "";
            for (int i = 0; i < comboBox.SelectedItem.ToString()!.Length; i++)
            {
                if (char.IsLetter(comboBox.SelectedItem.ToString()![i])) boundText += comboBox.SelectedItem.ToString()![i];
                else break;
            }

            editTextBox.BoundText = $"Enter new {boundText}";
            title.Text = $"Please choose a new {boundText}";
        }

        private void ReturnToOptionSelectionMenu()
        {
            btnEnter.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            viewBoxCombo.Visibility = Visibility.Visible;

            viewBoxEnter2.Visibility = Visibility.Collapsed;
            viewBoxReturn2.Visibility = Visibility.Collapsed;
            viewBoxTextBox.Visibility = Visibility.Collapsed;
            editTextBox.txtInput.Text = string.Empty;
            title.Text = "Please choose the option you would like to edit:";
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditBookmarkGrid2 = (Grid)mainWindow.FindName("EditBookmarkGrid2");
            EditBookmarkGrid2.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }
        private void ReturnToFirstMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditBookmarkGrid2 = (Grid)mainWindow.FindName("EditBookmarkGrid2");
            EditBookmarkGrid2.Visibility = Visibility.Collapsed;

            Grid EditBookmarkGrid = (Grid)mainWindow.FindName("EditBookmarkGrid");
            EditBookmarkGrid.Visibility = Visibility.Visible;
            comboBox.SelectedValue = null;
            comboBox.Text = "Option";
            comboBox.Foreground = Brushes.DarkGray;
        }

        private void EditArt()
        {
            string oldArt = bookmark.Isbn;
            try
            {
                bookmark.IsArtValid(editTextBox.txtInput.Text);
                DataBase.IsArtAvailable(editTextBox.txtInput.Text);
                bookmark.Isbn = editTextBox.txtInput.Text;
                DataBase.SaveItemInformation(bookmark);
                MessageBox.Show("The ART of the bookmark has been successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (IllegalArtException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                bookmark.Isbn = oldArt;
                editTextBox.txtInput.Focus();
            }
        }

        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditName()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Field id empty!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldName = bookmark.Name;
                try
                {
                    bookmark.Name = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(bookmark);
                    MessageBox.Show("The Name of the bookmark has been successfuly changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    bookmark.Name = oldName;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditEdition()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Field if empty!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldEdition = bookmark.Edition;
                try
                {
                    bookmark.Edition = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(bookmark);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    bookmark.Edition = oldEdition;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditQuantity()
        {
            int oldQuantity = bookmark.Quantity;
            try
            {
                bookmark.IsQuantityInt(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(bookmark);
                MessageBox.Show("The Quantity of the bookmark has been successfuly changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                bookmark.Quantity = oldQuantity;
                editTextBox.txtInput.Focus();
            }
        }

        private void EditPrice()
        {
            double oldPrice = bookmark.Price;
            try
            {
                bookmark.IsPriceDouble(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(bookmark);
                MessageBox.Show("The Price of the bookmark has been successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                bookmark.Price = oldPrice;
                editTextBox.txtInput.Focus();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null) comboBox.Foreground = Brushes.Black;
            else comboBox.Foreground = Brushes.DarkGray;
        }
    }
}