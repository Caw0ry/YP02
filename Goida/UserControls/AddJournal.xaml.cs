using System.Windows;
using System.Windows.Controls;
using System;
using GoidaLibrary;
using System.IO;
namespace Goida.UserControls
{
    public partial class AddJournal : UserControl
    {
        Journal journal = new Journal();
        public AddJournal()
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
                AssignJournalProperties();
                journal.IsFormValid();
                journal.IsIsbnValid(isbnx.txtInput.Text);
                journal.IsPriceDouble(pricex.txtInput.Text);
                journal.IsQuantityInt(quantityx.txtInput.Text);
                Journal actualJournal = new Journal(journal.Isbn, journal.Name, journal.Edition, journal.Quantity, journal.Price);
                DataBase.AddItem(actualJournal);
                DataBase.SaveItemInformation(actualJournal);
                MessageBox.Show($"{actualJournal.Name} has been successfully created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                DataBase.RemoveItem(isbnx.txtInput.Text);
            }
        }
        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ClearAllTextBoxes()
        {
            isbnx.txtInput.Clear();
            namex.txtInput.Clear();
            editionx.txtInput.Clear();
            quantityx.txtInput.Clear();
            pricex.txtInput.Clear();
        }
        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid AddJournalGrid = (Grid)mainWindow.FindName("AddJournalGrid");
            AddJournalGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            ClearAllTextBoxes();
        }
        private void AssignJournalProperties()
        {
            journal.Isbn = isbnx.txtInput.Text;
            journal.Name = namex.txtInput.Text;
            journal.Edition = editionx.txtInput.Text;
            journal.DateOfPrint = DateTime.Now;
        }
    }
}