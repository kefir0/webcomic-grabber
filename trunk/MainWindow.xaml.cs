#region Usings

using System.Windows;
using ComicGrabber.Grabbers;
using ComicGrabber.Models;

#endregion

namespace ComicGrabber
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow
   {
      #region Constructors

      public MainWindow()
      {
         InitializeComponent();
         DataContext = new GrabberViewModel();
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the Click event of the Button control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
      private void Button_Click(object sender, RoutedEventArgs e)
      {
         ((GrabberViewModel) DataContext).Export();
      }

      /// <summary>
      /// Handles the Click event of the ClearCacheButton control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
      private void ClearCacheButton_Click(object sender, RoutedEventArgs e)
      {
         TaskParallelGrabber.ClearCache();
      }

      #endregion
   }
}