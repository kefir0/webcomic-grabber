#region Usings

using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ComicGrabber.Helpers;
using ComicGrabber.Models;

#endregion


namespace ComicGrabber.Grabbers
{
   public abstract class TaskParallelGrabber : IGrabber
   {
      #region IGrabber implementation

      public abstract int GetCount();


      public IEnumerable<Comic> GetComics()
      {
         var count = GetCount();
         var tasks = Enumerable.Range(0, count).Select(GetTask).ToList();

         while (tasks.Count > 0) // Iterate until all tasks complete
         {
            var task = tasks.WaitAnyAndPop();
            if (task.Result != null) yield return task.Result;
         }
      }

      #endregion


      #region Public methods

      public override string ToString()
      {
         return GetType().Name;
      }


      /// <summary>
      /// Clears the isolated storage cache.
      /// </summary>
      public static void ClearCache()
      {
         foreach (var fileName in IsolatedStorageFile.GetMachineStoreForAssembly().GetFileNames())
         {
            IsolatedStorageFile.GetMachineStoreForAssembly().DeleteFile(fileName);
         }
      }

      #endregion


      #region Private and protected properties and indexers

      protected int RetryCount { get; set; }
      protected bool DisableCaching { get; set; }

      #endregion


      #region Private and protected methods

      private Task<Comic> GetTask(int index)
      {
         var task = new Task<Comic>(o =>
                                       {
                                          for (var i = 0; i < RetryCount + 1; i++) // Do some retries in case of network instability, etc
                                          {
                                             try
                                             {
                                                return DisableCaching ? GetComicByIndex((int) o) : GetCachedComicByIndex((int) o);
                                             }
                                             catch
                                             {
                                                continue; // See about task exception handling: http://msdn.microsoft.com/en-us/library/dd997415.aspx
                                             }
                                          }
                                          return null;
                                       }, index);
         task.Start();
         return task;
      }


      protected abstract Comic GetComicByIndex(int index);


      private Comic GetCachedComicByIndex(int index)
      {
         var f = IsolatedStorageFile.GetMachineStoreForAssembly();
         var fileName = GetType().FullName + index; // Use current grabber type plus index as a caching key

         if (f.FileExists(fileName)) // Return from cache
         {
            using (var stream = f.OpenFile(fileName, FileMode.Open))
            {
               return new DataContractSerializer(typeof (Comic)).ReadObjectSafe(stream) as Comic;
            }
         }

         var comic = GetComicByIndex(index); // Download and cache
         using (var stream = f.OpenFile(fileName, FileMode.Create))
         {
            if (comic != null)
            {
               new DataContractSerializer(typeof (Comic)).WriteObject(stream, comic);
            }
         }

         return comic;
      }

      #endregion
   }
}