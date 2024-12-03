using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using GoidaLibrary;
using System.IO;
using Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;
namespace Goida.UserControls
{
    public partial class EditJournal2 : UserControl
    {
        Journal journal = new Journal();
        public EditJournal2()
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
                comboBox.Items.Add($"ISBN ({journal.Isbn})");
                comboBox.Items.Add($"Name ({journal.Name})");
                comboBox.Items.Add($"Edition ({journal.Edition})");
                comboBox.Items.Add($"Quantity ({journal.Quantity})");
                comboBox.Items.Add($"Price ({journal.Price})");
            }
        }

        public void ClearComboBox()
        {
            comboBox.Items.Clear();
        }

        public void FindJournal()
        {
            try
            {
                journal = new Journal();
                journal = (Journal)DataBase.FindItem(EditJournal.Isbn!);
            }
            catch (InvalidCastException ex)
            {
                DataBase.LogException(ex);
                ReturnToFirstMenu();
                MessageBox.Show("The ISBN you entered to a book!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
                EditIsbn();
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
            editTextBox.txtInput.Text = string.Empty;
            title.Text = "Please choose the option you would like to edit:";
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid2 = (Grid)mainWindow.FindName("EditJournalGrid2");
            EditJournalGrid2.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }
        private void ReturnToFirstMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid2 = (Grid)mainWindow.FindName("EditJournalGrid2");
            EditJournalGrid2.Visibility = Visibility.Collapsed;

            Grid EditJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
            EditJournalGrid.Visibility = Visibility.Visible;
            comboBox.SelectedValue = null;
            comboBox.Text = "Option";
            comboBox.Foreground = Brushes.DarkGray;
        }

        private void EditIsbn()
        {
            string oldIsbn = journal.Isbn;
            try
            {
                journal.IsIsbnValid(editTextBox.txtInput.Text);
                DataBase.IsIsbnAvailable(editTextBox.txtInput.Text);
                journal.Isbn = editTextBox.txtInput.Text;
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("The ISBN of the journal has been successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (IllegalIsbnException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                journal.Isbn = oldIsbn;
                editTextBox.txtInput.Focus();
            }
        }

        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "ERROE", MessageBoxButton.OK, MessageBoxImage.Error);
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
                string oldName = journal.Name;
                try
                {
                    journal.Name = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(journal);
                    MessageBox.Show("The Name of the journal has been successfuly changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    journal.Name = oldName;
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
                string oldEdition = journal.Edition;
                try
                {
                    journal.Edition = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(journal);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    journal.Edition = oldEdition;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditQuantity()
        {
            int oldQuantity = journal.Quantity;
            try
            {
                journal.IsQuantityInt(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("The Quantity of the journal has been successfuly changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                journal.Quantity = oldQuantity;
                editTextBox.txtInput.Focus();
            }
        }
        private void EditPrice()
        {
            double oldPrice = journal.Price;
            try
            {
                journal.IsPriceDouble(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("The Price of the journal has been successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                journal.Price = oldPrice;
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