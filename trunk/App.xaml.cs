#region Usings

using System.Windows;
using System.Windows.Threading;

#endregion

namespace ComicGrabber
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App
   {
      #region Event handlers

      /// <summary>
      /// Handles the DispatcherUnhandledException event of the Application control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
      private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
      {
         MessageBox.Show(e.Exception.ToString());
         e.Handled = true;
      }

      #endregion
   }
}