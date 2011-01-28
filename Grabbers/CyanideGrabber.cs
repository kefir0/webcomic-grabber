#region Usings

using System.Net;
using ComicGrabber.Helpers;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber.Grabbers
{
   /// <summary>
   /// Cyanide And Happiness comic grabber.
   /// </summary>
   public class CyanideGrabber : TaskParallelGrabber
   {
      #region Fields and Constants

      public const string ImageToken = "alt=\"Cyanide and Happiness, a daily webcomic\" src=\"";
      public const int Offset = 15;
      public const string UrlFormatString = "http://www.explosm.net/comics/{0}/";

      #endregion

      #region Public methods

      public override int GetCount()
      {
         return 2400; // TODO: Fetch real number
      }

      #endregion

      #region Private and protected methods

      protected override Comic GetComicByIndex(int index)
      {
         var url = string.Format(UrlFormatString, index + Offset);
         var html = new WebClient().DownloadString(url);

         var startIndex = html.IndexOf(ImageToken);
         if (startIndex < 0) return null;

         var endIndex = html.IndexOf('"', startIndex + ImageToken.Length);
         if (endIndex < 0) return null;

         startIndex += ImageToken.Length;
         var imageUrl = html.Substring(startIndex, endIndex - startIndex);

         // Download picture
         var imageStream = WebRequest.Create(imageUrl).GetResponse().GetResponseStream().ToMemoryStream();

         var comic = Comic.Create(imageStream.GetBuffer());
         if (comic != null)
         {
            comic.Url = url;
            comic.Index = index;
            return comic;
         }

         return null;
      }

      #endregion
   }
}