using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;
namespace Goida.UserControls
{
    public partial class EditBookmark : UserControl
    {
        public static string? Art { get; set; }
        public EditBookmark()
        {
            InitializeComponent();

            btnEnter.ButtonContent = Resex.btnEnter;
            btnreturn.ButtonContent =Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnreturn.ButtonClickEvent += btnInput_Click;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnEnter)
            {
                try
                {
                    DataBase.IsIsbnValid(artBox.txtInput.Text);
                    Art = artBox.txtInput.Text;
                    ProceedToNextMenu();
                }
                catch (IllegalIsbnException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    artBox.txtInput.Focus();
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
            Grid EditBookmarkGrid = (Grid)mainWindow.FindName("EditBookmarkGrid");
            EditBookmarkGrid.Visibility = Visibility.Collapsed;

            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            artBox.txtInput.Text = string.Empty;
        }

        private void ProceedToNextMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditBookmarkGrid = (Grid)mainWindow.FindName("EditBookmarkGrid");
            EditBookmarkGrid.Visibility = Visibility.Collapsed;
            
            Grid EditBookmarkGrid2 = (Grid)mainWindow.FindName("EditBookmarkGrid");
            EditBookmarkGrid2.Visibility = Visibility.Visible;

            EditBookmark2 editBookmark2Control = (EditBookmark2)EditBookmarkGrid2.Children[0];
            editBookmark2Control.ClearComboBox();
            editBookmark2Control.FindBookmark();
            editBookmark2Control.PopulateComboBox();
        }
    }
}