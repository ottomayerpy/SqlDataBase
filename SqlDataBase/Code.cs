using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SqlDataBase
{
    internal class Code
    {
        public static BitmapImage LoadImage(byte[] imageData)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                {
                    return null;
                }

                BitmapImage image = new BitmapImage();

                using (MemoryStream mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string DateFormatToSqlServer(string str)
        {
            DateTime date = DateTime.ParseExact(str, "dd.MM.yyyy", null);
            return date.Date.ToString("yyyy-MM-dd");
        }
    }
}
