#region Usings

using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ComicGrabber.Helpers;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber.Grabbers
{
   /// <summary>
   /// brainlesstales.com grabber. There is roughly one strip each day starting from 2007-05-07.
   /// </summary>
   public class BrainlessTalesGrabber : TaskParallelGrabber
   {
      #region Fields and Constants

      public const string UrlFormatString = "http://www.brainlesstales.com/{0}/";  // {0} is date: from 2007-05-07 to today.
      private int _count = -1;
      private static readonly DateTime StartDate = new DateTime(2007, 5, 7);

      #endregion

      #region Public methods

      public override int GetCount()
      {
         if (_count < 0)
         {
            try
            {
               _count = (int) (DateTime.Now - StartDate).TotalDays;
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
         var comicDate = StartDate.AddDays(index);
         var dateString = comicDate.ToString("yyyy-MM-dd"); //2007-05-07
         var comicUrl = string.Format(UrlFormatString, dateString);
         var comicInfo = wc.DownloadString(comicUrl);

         var pictureUrl = Regex.Match(comicInfo, string.Format(@"http://www.brainlesstales.com/images/{0}/.*\.jpg", comicDate.Year)).Value;
         var title = GetStringBetween(comicInfo, "<title>", " - Brainless Tales</title>");

         // Download picture
         var imageStream = WebRequest.Create(pictureUrl).GetResponse().GetResponseStream().ToMemoryStream();
         var comic = Comic.Create(imageStream.GetBuffer());
         if (comic == null) return null;

         comic.Index = index + 1;
         comic.Title = string.Format("{0} ({1})", title, dateString);
         //comic.Description = dateString;
         comic.Url = comicUrl;

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