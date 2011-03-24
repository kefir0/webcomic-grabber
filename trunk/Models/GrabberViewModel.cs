#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ComicGrabber.Grabbers;

#endregion

namespace ComicGrabber.Models
{
   public class GrabberViewModel : ModelBase
   {
      #region Fields and Constants

      private readonly ObservableCollection<Comic> _comics = new ObservableCollection<Comic>();
      private readonly Dispatcher _dispatcher;
      private bool _isExporting;
      private int _maxProgress;

      #endregion

      #region Constructors

      public GrabberViewModel()
      {
         _dispatcher = Dispatcher.CurrentDispatcher;
         StartGrabbing();
      }

      #endregion

      #region Public properties and indexers

      public ObservableCollection<Comic> Comics
      {
         get { return _comics; }
      }


      public int MaxProgress
      {
         get { return _maxProgress; }
         set
         {
            _maxProgress = value;
            OnPropertyChanged("MaxProgress");
         }
      }


      public bool IsExporting
      {
         get { return _isExporting; }
         set
         {
            _isExporting = value;
            OnPropertyChanged("IsExporting");
         }
      }

      #endregion

      #region Public methods

      public void Export()
      {
         if (IsExporting) return;
         IsExporting = true;
         ThreadPool.QueueUserWorkItem(o =>
                                         {
                                            try
                                            {
                                               var fileName = "export.pdf";
                                               PdfExporter.Export(Comics, fileName);
                                               Process.Start(fileName);  // Open resulting file in associated application
                                            }
                                            catch
                                            {
                                            }
                                            finally
                                            {
                                               IsExporting = false;
                                            }
                                         });
      }

      #endregion

      #region Private and protected methods

      private void StartGrabbing()
      {
         ThreadPool.QueueUserWorkItem(o => DoGrabbing());
      }


      private void DoGrabbing()
      {
         var grabber = new BrainlessTalesGrabber();
         //var grabber = new FolderGrabber(@"D:\Photo\Bike\2008-03-07 Zavod\proc");
         //var grabber = new CyanideGrabber();
         //var grabber = new WtdGrabber();

         MaxProgress = grabber.GetCount();

         foreach (var comic in grabber.GetComics())
         {
            var c = comic;
            _dispatcher.Invoke((Action) (() => Comics.Add(c)), DispatcherPriority.ApplicationIdle);
         }

         MaxProgress = Comics.Count;  // Show completed progressbar.
         Export();
      }

      #endregion
   }
}