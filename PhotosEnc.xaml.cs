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
    /// Interaction logic for PhotosEnc.xaml
    /// </summary>
    public partial class PhotosEnc : Page
    {
        public PhotosEnc()
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
                Filter = "Image Files (.jpg)|*.jpg;*.jpeg;*.png;*.gif;*.tif;"// Filter files by extension
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


                    string outFile = imageUri.LocalPath.Split('.')[0] + "Enc";

                    byte[] EncryptedPic = Crypt.Encrypt(vs);

                    byte[] DecryptedPic = Crypt.Decrypt(EncryptedPic);

                    File.WriteAllBytes(outFile, EncryptedPic);

                    picEnc.Source = ToImage(DecryptedPic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }


        private void BtnDec_Click(object sender, RoutedEventArgs e)
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
                    byte[] DecryptedPic = Crypt.Decrypt(temp);
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

        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Crypt.mode = (sender as ComboBox).SelectedIndex switch
                {
                    0 => CipherMode.ECB,
                    1 => CipherMode.CBC,
                    _ => throw new Exception("un Vaild!"),
                };            
                if(IsInitialized){
                    modeDec.SelectedIndex = Crypt.mode ==CipherMode.ECB ? 0 : 1;
                    modeEnc.SelectedIndex = Crypt.mode == CipherMode.ECB ? 0 : 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }

        #region Image Covert
        /// <summary>
        /// convert image for a form to deal with cryptography 
        /// </summary>
        
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

    }
}
