using System.ComponentModel;


namespace ComicGrabber.Models
{
   public class ModelBase : INotifyPropertyChanged
   {
      #region INotifyPropertyChanged implementation

      public event PropertyChangedEventHandler PropertyChanged;

      #endregion


      #region Public methods

      public void OnPropertyChanged(string propertyName)
      {
         var handler = PropertyChanged;
         if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion
   }
}