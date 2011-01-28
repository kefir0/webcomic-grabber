#region Usings

using System.IO;
using System.Runtime.Serialization;

#endregion

namespace ComicGrabber.Helpers
{
   public static class DataContractSerializerExtensions
   {
      public static object ReadObjectSafe(this DataContractSerializer serializer, Stream stream)
      {
         try
         {
            return serializer.ReadObject(stream);
         }
         catch
         {
            return null;
         }
      }
   }
}