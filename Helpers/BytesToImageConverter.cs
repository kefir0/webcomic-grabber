#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#endregion

namespace ComicGrabber.Helpers
{
   public class BytesToImageConverter : IValueConverter
   {
      #region IValueConverter implementation

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var bytes = value as byte[];
         if (bytes == null) return null;

         var bmp = new BitmapImage();
         bmp.BeginInit();
         bmp.StreamSource = new MemoryStream(bytes);
         bmp.EndInit();
         return bmp;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}