using System.Windows;
using System.Windows.Controls;
namespace Goida.UserControls
{
    public partial class MenuButton : UserControl
    {
        public MenuButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string), typeof(MenuButton), new PropertyMetadata("Button"));

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public event RoutedEventHandler ButtonClickEvent;

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            ButtonClickEvent?.Invoke(this, e);
        }
    }
}
