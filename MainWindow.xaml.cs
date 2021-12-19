
using System.Windows;


namespace EncryptUsingAES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new PhotosEnc();
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            Main.Content = new PhotosEnc();
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Main.Content = new textEnc();
        }
    }

}
