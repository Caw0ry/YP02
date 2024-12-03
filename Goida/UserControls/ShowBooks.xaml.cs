using GoidaLibrary;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using Range = Microsoft.Office.Interop.Word.Range;
namespace Goida.UserControls
{
    public partial class ShowBooks : UserControl // определение класса отображения имеющихся книг
    {
        public ShowBooks()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnFind.ButtonContent = Resex.btnFind;
            btnRevert.ButtonContent = Resex.btnShowBooks;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            btnFind.ButtonClickEvent += BtnFind_ButtonClickEvent;
            btnRevert.ButtonClickEvent += BtnRevert_ButtonClickEvent;
            DisplayBooks();
        }

        private void BtnFind_ButtonClickEvent(object sender, RoutedEventArgs e) // обработчик события кнопки поиска по имени
        {
            if (textBox.txtInput.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name in the text box in order to sort the list!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    var bookList = DataBase.FilterItemsByName(textBox.txtInput.Text);
                    var itemsList = bookList;
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        if (itemsList[i].GetType() == typeof(Journal)) bookList.Remove(itemsList[i]);
                        if (bookList.Count == 0)
                        {
                            MessageBox.Show("The name you entered does not relate to any of the books in the library!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    listBox.ItemsSource = null;
                    listBox.ItemsSource = bookList;
                }
                catch (System.ArgumentException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Ссылок: 1
        private void BtnRevert_ButtonClickEvent(object sender, RoutedEventArgs e) //обработчик события обновления списка
        {
            DisplayBooks();
        }

        // Ссылок: 3
        public void DisplayBooks() //метод отображения книг
        {
            listBox.ItemsSource = null;
            listBox.ItemsSource = DataBase.GetBooks();
        }

        // Ссылок: 1
        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e) //обработчик события кнопки возврата
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            Grid ShowBooksGrid = (Grid)mainWindow.FindName("ShowBooksGrid");
            ShowBooksGrid.Visibility = Visibility.Collapsed;
        }

        // Ссылок: 1
        private void BtnWord_ButtonClickEvent(object sender, RoutedEventArgs e) // обработчик события кнопки вывода информации в Word
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = true;
                Document doc = wordApp.Documents.Add();
                Range range = doc.Content;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertAfter(title.Text + "\n\n");
                List<AbstractItem> books = DataBase.GetBooks();
                foreach (AbstractItem item in books)
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
    }
}
