#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

#endregion

namespace ComicGrabber.Models
{
   public static class PdfExporter
   {
      #region Public methods

      public static void Export(IEnumerable<Comic> comics, string fileName)
      {
         const float margin = 5;
         const float textSize = 16;
         var document = new Document(PageSize.LETTER, margin, margin, margin, margin);

         // Support for cyrillic charset. Path looks bad, need to find out how to improve.
         //var baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", System.Text.Encoding.GetEncoding(1251).BodyName, true);
         var baseFont = Font.FontFamily.TIMES_ROMAN;
         var titleFont = new Font(baseFont, textSize + 2, Font.BOLD);
         var altFont = new Font(baseFont, textSize, Font.ITALIC);

         try
         {
            File.Delete(fileName);
            var wri = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

            //Open Document to write
            document.Open();
            document.Add(new Paragraph()); // Fix initial spacing

            //Write some content
            foreach (var comic in comics.OrderBy(c => c.Index).ToList())
            {
               var image = Image.GetInstance(new MemoryStream(comic.ImageBytes));

               if (string.IsNullOrEmpty(comic.Title) && string.IsNullOrEmpty(comic.Description))
               {
                  // No text - fill the document
                  image.ScaleToFit(document.PageSize.Width - margin*2, document.PageSize.Height - margin*2 - 20);
                  document.Add(image);
               }
               else
               {
                  var title = new Paragraph(comic.Index + ". " + comic.Title, titleFont);
                  title.SetAlignment("Center");
                  document.Add(title);

                  // Measure vertical space needed for text: count number of lines needed
                  var textLines = (int) Math.Floor((new Chunk(comic.Description).GetWidthPoint()/document.PageSize.Width)) + 3;

                  // Fit image to page size
                  image.ScaleToFit(document.PageSize.Width - margin*2, document.PageSize.Height - textLines*textSize*2 - margin*2);

                  document.Add(image);
                  document.Add(new Phrase(comic.Description, altFont));
                  document.Add(Chunk.NEXTPAGE);
               }
            }
         }
         catch
         {
         }
         finally
         {
            document.Close(); //Close document
         }
      }

      #endregion
   }
}