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
   public class WtdGrabber : TaskParallelGrabber
   {
      #region Fields and Constants

      public const int Offset = 1;
      public const string UrlFormatString = "http://www.whattheduck.net/sites/default/files/WTD{0}.gif";

      #endregion

      #region Public methods

      public override int GetCount()
      {
         return 1200; // TODO: Fetch real number
      }

      #endregion

      #region Private and protected methods

      protected override Comic GetComicByIndex(int index)
      {
         var comicIndex = index + Offset;
         var number = comicIndex.ToString("0#");  // Add 0 to one-digit numbers

         while (true)
         {
            var imageUrl = string.Format(UrlFormatString, number);

            // Download picture
            var imageStream = WebRequest.Create(imageUrl).GetResponse().GetResponseStream().ToMemoryStream();

            var comic = Comic.Create(imageStream.GetBuffer());
            if (comic != null)
            {
               comic.Index = comicIndex;
               return comic;
            }

            if (number.Contains("_")) return null;
            number += "_0"; // Some comics are like 01_0, try that
         }
      }

      #endregion
   }
}