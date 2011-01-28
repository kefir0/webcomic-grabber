using System.Collections.Generic;
using ComicGrabber.Models;

namespace ComicGrabber.Grabbers
{
   public interface IGrabber
   {
      int GetCount();
      IEnumerable<Comic> GetComics();
   }
}