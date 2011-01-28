#region Usings

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace ComicGrabber.Helpers
{
   internal class NumberToBooleanConverter : IValueConverter
   {
      #region IValueConverter implementation

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null) return false;
         return (System.Convert.ToInt32(value) > 0) ^ (parameter != null);
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}