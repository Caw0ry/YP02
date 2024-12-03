using GoidaLibrary;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Range = Microsoft.Office.Interop.Word.Range;
namespace Goida.UserControls
{
    public partial class Receipts : UserControl //определение класса чеков
    {
        public Receipts()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            DisplayReceipts();
        }

        private void BtnWord_ButtonClickEvent(object sender, RoutedEventArgs e) //обработчик события вывода информации в Word
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = true;
                Document doc = wordApp.Documents.Add();
                Range range = doc.Content;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertAfter(title.Text + "\n\n");
                List<string> items = DataBase.GetReceipts();
                foreach (string item in items)
                {
                    range.InsertAfter(item.ToString() + "\n\n");
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e) //обработчик события кнопки возврата
        {
            ReturnToMenu();
        }

        private void ReturnToMenu() //метод возврата в меню менеджера
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid ReceiptsGrid = (Grid)mainWindow.FindName("ReceiptsGrid");
            ReceiptsGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        public void DisplayReceipts() //метод отображения чеков
        {
            try
            {
                listBox.ItemsSource = null;
                listBox.ItemsSource = DataBase.GetReceipts();
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
