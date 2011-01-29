#region Usings

using System.IO;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber.Grabbers
{
   /// <summary>
   /// Grab files from local folder.
   /// </summary>
   public class FolderGrabber : TaskParallelGrabber
   {
      #region Constructors

      public FolderGrabber()
      {
         DisableCaching = true;
         RetryCount = 0;
      }

      #endregion

      #region Public properties and indexers

      public string FolderPath { get; set; }

      #endregion

      #region Public methods

      public override int GetCount()
      {
         return Directory.GetFiles(FolderPath).Length;
      }

      #endregion

      #region Private and protected methods

      protected override Comic GetComicByIndex(int index)
      {
         return Comic.Create(File.ReadAllBytes(Directory.GetFiles(FolderPath)[index]));
      }

      #endregion
   }
}