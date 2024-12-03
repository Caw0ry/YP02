using GoidaLibrary;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.IO;
using Range = Microsoft.Office.Interop.Word.Range;

namespace Goida.UserControls
{
    public partial class ShowAvailableItems: UserControl //определение класса отображения имеющихся книг
    {
        
        public ShowAvailableItems()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnFind.ButtonContent = Resex.btnFind;
            btnRevert.ButtonContent = Resex.btnShowItems;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            btnFind.ButtonClickEvent += BtnFind_ButtonClickEvent;
            btnRevert.ButtonClickEvent += BtnRevert_ButtonClickEvent;
            DisplayItems();
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
                    DataBase.FilterItemsByName(textBox.txtInput.Text);
                    listBox.ItemsSource = null;
                    listBox.ItemsSource = DataBase.FilterItemsByName(textBox.txtInput.Text);
                }
                catch (System.ArgumentException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnRevert_ButtonClickEvent(Object sender, RoutedEventArgs e)
        {
            DisplayItems();
        }

        public void DisplayItems()
        {
            listBox.ItemsSource = null ;
            listBox.ItemsSource = DataBase.GetItems();
        }
        
        private void BtnReturn_ButtonClickEvent(Object sender, RoutedEventArgs e)
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            Grid ShowAvailableItemsGrid = (Grid)mainWindow.FindName("ShowAvailableItemsGrid");
            ShowAvailableItemsGrid.Visibility = Visibility.Collapsed;
        }

        private void  BtnWord_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application receiptsWordApp = new Microsoft.Office.Interop.Word.Application();
                receiptsWordApp.Visible = true;
                Document doc = receiptsWordApp.Documents.Add();
                Range range = doc.Content;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;


                range.InsertAfter(title.Text + "\n\n");
                List<AbstractItem> items = DataBase.GetItems();
                foreach (AbstractItem item in items)
                {
                    range.InsertAfter(item.ToString() + "\n\n");
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(receiptsWordApp);
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }
    }
}