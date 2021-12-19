using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Show message box when button is clicked.
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new()
            {
                FileName = "Document", // Default file name
                DefaultExt = ".png", // Default file extension
                Filter = "Photos(.png)|*.png" // Filter files by extension
            };

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                Uri imageUri = new(filename);

                BitmapImage imageBitmap = new(imageUri);
                //pic.Source = imageBitmap;
                try
                {
                    byte[] vs = ToByteArray(BitmapImage2Bitmap(imageBitmap));

                    picEnc.Source = ToImage(vs);
                 
                         
                    string outFile = imageUri.LocalPath.Split('.')[0] + "Enc"; // @"C:\Users\shukri\Desktop\enc";

                    byte[] EncryptedPic = Encrypt(vs);

                    byte[] DecryptedPic = Decrypt(EncryptedPic);

                    File.WriteAllBytes(outFile, EncryptedPic);

                    picEnc.Source = ToImage(DecryptedPic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }


        private void btnDec_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new()
            {
                FileName = "Encrypted File", // Default file name
            };

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                try
                {
                    // Open document
                    string filename = dlg.FileName;
                    byte[] temp = File.ReadAllBytes(filename);
                    Uri imageUri = new(filename);
                    byte[] DecryptedPic = Decrypt(temp);
                    string outFile = imageUri.LocalPath.Split('.')[0] + "Dec.png";
                    File.WriteAllBytes(outFile, DecryptedPic);
                    picDec.Source = ToImage(DecryptedPic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }


        private void Encrypt_txt_Click(object sender, RoutedEventArgs e)
        {
            if (plainText.Text != null)
            {
                try
                {

                    var temp = System.Text.ASCIIEncoding.ASCII.GetBytes(plainText.Text);
                    CipherText.Text = Convert.ToBase64String(Encrypt(temp));
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
                    plainText.Text = System.Text.ASCIIEncoding.ASCII.GetString(Decrypt(temp));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                switch ((sender as ComboBox).SelectedIndex)
                {
                    case 0:
                        mode = CipherMode.ECB;
                        break;
                    case 1:
                        mode = CipherMode.CBC;
                        break;
                    case 2:
                        mode = CipherMode.CTS;
                        break;
                    default:
                        throw new Exception("un Vaild!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        #region Image Covert
        public static BitmapImage ToImage(byte[] array)
        {
            using MemoryStream ms = new(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // here
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using MemoryStream outStream = new();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            Bitmap bitmap = new(outStream);

            return new Bitmap(bitmap);
        }

        public static byte[] ToByteArray(Bitmap bitmap)
        {
            MemoryStream stream = new();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] byteArray = stream.GetBuffer();
            return byteArray;
        }
        #endregion



        #region AES   
        //generate key
        public static readonly byte[] key = Aes.Create().Key;
        //generate IV
        public static readonly byte[] IV = Aes.Create().IV;
        //set mode
        static CipherMode mode = CipherMode.ECB;

        private static byte[] Encrypt(byte[] plain)
        {

            AesCryptoServiceProvider aes = new()
            {
                Key = key,
                Mode = mode,
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.Zeros
            };

            ICryptoTransform crypto = aes.CreateEncryptor(key, IV);
            byte[] cipher = crypto.TransformFinalBlock(plain, 0, plain.Length);
            crypto.Dispose();
            return cipher;
        }

        private static byte[] Decrypt(byte[] cipher)
        {
            AesCryptoServiceProvider aes = new()
            {
                Key = key,
                Mode = mode,
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.Zeros
            };
            ICryptoTransform crypto = aes.CreateDecryptor(key, IV);
            byte[] plain = crypto.TransformFinalBlock(cipher, 0, cipher.Length);
            crypto.Dispose();
            return plain;
        }
        #endregion
    }

}
