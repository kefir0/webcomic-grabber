#region Usings

using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

#endregion

namespace ComicGrabber.Models
{
   [DataContract]
   public class Comic
   {
      #region Constructors

      private Comic()
      {
      }

      #endregion

      #region Public properties and indexers

      private byte[] _imageBytes;

      [DataMember]
      public byte[] ImageBytes
      {
         get { return _imageBytes; }
         private set
         {
            _imageBytes = value;
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.DecodePixelHeight = 100; // Do not store whole picture
            bmp.StreamSource = new MemoryStream(_imageBytes);
            bmp.EndInit();
            bmp.Freeze();
            Thumbnail = bmp;
         }
      }

      public BitmapImage Thumbnail { get; private set; }

      [DataMember]
      public string Title { get; set; }

      [DataMember]
      public string Description { get; set; }

      [DataMember]
      public int Index { get; set; }

      [DataMember]
      public string Url { get; set; }

      #endregion

      #region Public methods

      public static Comic Create(byte[] imageBytes)
      {
         try
         {
            // Validate image bytes by trying to create a Thumbnail.
            return new Comic {ImageBytes = imageBytes};
         }
         catch
         {
            // Failure, cannot decode bytes
            return null;
         }
      }

      #endregion
   }
}