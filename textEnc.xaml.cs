using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EncryptUsingAES
{
    /// <summary>
    /// Interaction logic for textEnc.xaml
    /// </summary>
    public partial class textEnc : Page
    {
        public textEnc()
        {
            InitializeComponent();
        }
        private void Encrypt_txt_Click(object sender, RoutedEventArgs e)
        {
            if (plainText.Text != null)
            {
                try
                {

                    var temp = Encoding.ASCII.GetBytes(plainText.Text);
                    CipherText.Text = Convert.ToBase64String(Crypt.Encrypt(temp));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Decrypt_txt_Click(object sender, RoutedEventArgs e)
        {

            if (CipherText.Text != null)
            {
                try
                {
                    var temp = Convert.FromBase64String(CipherText.Text);
                    plainText.Text = Encoding.ASCII.GetString(Crypt.Decrypt(temp));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
