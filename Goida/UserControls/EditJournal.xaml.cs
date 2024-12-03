using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;
namespace Goida.UserControls
{
    public partial class EditJournal : UserControl
    {
        public static string? Isbn { get; set; }
        public EditJournal()
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
                    DataBase.IsIsbnValid(isbnBox.txtInput.Text);
                    Isbn = isbnBox.txtInput.Text;
                    ProceedToNextMenu();
                }
                catch (IllegalIsbnException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    isbnBox.txtInput.Focus();
                }
            }
            else
            {
                ReturnToManagerMenu();
            }
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
            EditJournalGrid.Visibility = Visibility.Collapsed;

            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            isbnBox.txtInput.Text = string.Empty;
        }

        private void ProceedToNextMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
            EditJournalGrid.Visibility = Visibility.Collapsed;
            
            Grid EditJournalGrid2 = (Grid)mainWindow.FindName("EditJournalGrid2");
            EditJournalGrid2.Visibility = Visibility.Visible;

            EditJournal2 EditJournal2Control = (EditJournal2)EditJournalGrid2.Children[0];
            EditJournal2Control.ClearComboBox();
            EditJournal2Control.FindJournal();
            EditJournal2Control.PopulateComboBox();
        }
    }
}