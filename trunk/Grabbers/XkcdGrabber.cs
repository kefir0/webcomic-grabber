#region Usings

using System.Net;
using System.Runtime.Serialization.Json;
using ComicGrabber.Helpers;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber.Grabbers
{
   internal class XkcdGrabber : TaskParallelGrabber
   {
      #region Fields and Constants

      public const string LastComicUrl = "http://xkcd.com/info.0.json";
      public const string UrlFormatString = "http://xkcd.com/{0}/info.0.json";
      private int _count = -1;

      #endregion

      #region Public methods

      public override int GetCount()
      {
         if (_count < 0)
         {
            try
            {
               // Get last comic number by checking root page
               var lastComic = GetComic(LastComicUrl);
               _count = lastComic != null ? int.Parse(lastComic.num) : 0;
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
         // Download comic JSON
         var comicInfo = GetComic(string.Format(UrlFormatString, index + 1));
         if (comicInfo == null) return null;

         // Download picture
         var imageStream = WebRequest.Create(comicInfo.img).GetResponse().GetResponseStream().ToMemoryStream();
         var comic = Comic.Create(imageStream.GetBuffer());		 
		 if (comic == null) return null;
		 
         comic.Description = comicInfo.alt;
         comic.Url = comicInfo.link;
         comic.Index = index + 1;
         comic.Title = comicInfo.title;

         return comic;
      }

      private static XkcdComic GetComic(string url)
      {
         var stream = new WebClient().OpenRead(url);
         if (stream == null) return null;
         var serializer = new DataContractJsonSerializer(typeof (XkcdComic));
         return serializer.ReadObject(stream) as XkcdComic;
      }

      #endregion
   }
}