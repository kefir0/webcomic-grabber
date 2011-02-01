#region Usings

using System.Net;
using System.Text;
using ComicGrabber.Helpers;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber.Grabbers
{
   public class RuXkcdGrabber : TaskParallelGrabber
   {
      #region Fields and Constants

      public const string UrlFormatString = "http://xkcd.ru/{0}/";
      private int _count = -1;

      #endregion

      #region Public methods

      public override int GetCount()
      {
         if (_count < 0)
         {
            try
            {
               _count = new XkcdGrabber().GetCount(); 
            }
            catch
            {
               _count = 0;
            }
         }
         return _count;
      }

      #endregion

      #region Private and protected methods

      protected override Comic GetComicByIndex(int index)
      {
         // Download comic HTML
         var wc = new WebClient {Encoding = Encoding.UTF8};
         var comicInfo = wc.DownloadString(string.Format(UrlFormatString, index + 1));
         var pictureUrl = GetStringBetween(comicInfo, "<img border=0 src=\"", "\" alt=");
         var title = GetStringBetween(comicInfo, "<h1>", "</h1>");
         var alt = GetStringBetween(comicInfo, "<div class=\"comics_text\">", "</div>");

         // Download picture
         var imageStream = WebRequest.Create(pictureUrl).GetResponse().GetResponseStream().ToMemoryStream();
         var comic = Comic.Create(imageStream.GetBuffer());
         if (comic == null) return null;

         comic.Description = alt;
         comic.Index = index + 1;
         comic.Title = title;

         return comic;
      }

      private static string GetStringBetween(string s, string start, string end)
      {
         var startIndex = s.IndexOf(start);
         if (startIndex < 0) return null;

         var endIndex = s.IndexOf(end, startIndex + start.Length);
         if (endIndex < 0) return null;

         startIndex += start.Length;
         return s.Substring(startIndex, endIndex - startIndex);
      }

      #endregion
   }
}