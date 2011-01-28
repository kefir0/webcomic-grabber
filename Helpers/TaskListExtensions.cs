using System.Collections.Generic;
using System.Threading.Tasks;


namespace ComicGrabber.Helpers
{
   public static class TaskListExtensions
   {
      #region Public methods

      /// <summary>
      /// Waits for any task to complete, removes it from list and returns.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="taskList">The task list.</param>
      /// <returns></returns>
      public static Task<T> WaitAnyAndPop<T>(this List<Task<T>> taskList)
      {
         var array = taskList.ToArray();
         var task = array[Task.WaitAny(array)];
         taskList.Remove(task); // No need to dispose task, see here: http://stackoverflow.com/questions/3734280/is-it-considered-acceptable-to-not-call-dispose-on-a-tpl-task-object
         return task;
      }

      #endregion
   }
}