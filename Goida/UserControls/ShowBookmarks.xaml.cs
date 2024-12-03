using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GoidaLibrary;
using Microsoft.Office.Interop.Word;
using Range = Microsoft.Office.Interop.Word.Range;

namespace Goida.UserControls
{
    public partial class ShowBookmarks : UserControl
    {
        public ShowBookmarks()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnFind.ButtonContent = Resex.btnFind;
            btnRevert.ButtonContent = Resex.btnShowBookmarks;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            btnFind.ButtonClickEvent += BtnFind_ButtonClickEvent;
            btnRevert.ButtonClickEvent += BtnRevert_ButtonClickEvent;
            DisplayBookmarks();
        }

        private void BtnFind_ButtonClickEvent(object sender, RoutedEventArgs e) 
        {
            if (textBox.txtInput.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name in the text box in order to sort the list!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    var bookmarkList = DataBase.FilterItemsByName(textBox.txtInput.Text);
                    var itemsList = bookmarkList;
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        if (itemsList[i].GetType() == typeof(Book))
                        {
                            bookmarkList.Remove(itemsList[i]);
                        }
                    }
                    if (bookmarkList.Count == 0)
                    {
                        MessageBox.Show("The name you entered does not relate to any of the bookmarks in the library!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    listBox.ItemsSource = null;
                    listBox.ItemsSource = bookmarkList;
                }
                catch (System.ArgumentException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnRevert_ButtonClickEvent(object sender, RoutedEventArgs e) 
        {
            DisplayBookmarks();
        }

        public void DisplayBookmarks() 
        {
            listBox.ItemsSource = null;
            listBox.ItemsSource = DataBase.GetBookmarks();
        }

        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e) 
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            Grid ShowBookmarksGrid = (Grid)mainWindow.FindName("ShowBookmarksGrid");
            ShowBookmarksGrid.Visibility = Visibility.Collapsed;
        }

        private void BtnWord_ButtonClickEvent(object sender, RoutedEventArgs e) 
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = true;
                Document doc = wordApp.Documents.Add();
                Range range = doc.Content;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertAfter(title.Text + "\n\n");
                List<AbstractItem> bookmarks = DataBase.GetBookmarks();
                foreach (AbstractItem item in bookmarks)
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
