#region Usings

using System.Runtime.Serialization;

#endregion

namespace ComicGrabber.Grabbers
{
   [DataContract]
   public class XkcdComic
   {
      #region Public properties and indexers

      [DataMember]
      public string img { get; set; }

      [DataMember]
      public string title { get; set; }

      [DataMember]
      public string month { get; set; }

      [DataMember]
      public string num { get; set; }

      [DataMember]
      public string link { get; set; }

      [DataMember]
      public string year { get; set; }

      [DataMember]
      public string news { get; set; }

      [DataMember]
      public string safe_title { get; set; }

      [DataMember]
      public string transcript { get; set; }

      [DataMember]
      public string day { get; set; }

      [DataMember]
      public string alt { get; set; }

      #endregion
   }
}