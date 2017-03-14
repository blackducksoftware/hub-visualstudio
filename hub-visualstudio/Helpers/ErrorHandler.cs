using System;
using Microsoft.VisualStudio.Shell;

namespace BlackDuckHub.VisualStudio.Helpers
{
    public static class TaskManager
    {
        //https://mhusseini.wordpress.com/2013/05/30/write-to-visual-studios-error-list/

        private static ErrorListProvider _errorListProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _errorListProvider = new ErrorListProvider(serviceProvider);
        }

        public static void AddError(string message)
        {
            AddTask(message, TaskErrorCategory.Error);
        }

        public static void AddWarning(string message)
        {
            AddTask(message, TaskErrorCategory.Warning);
        }

        public static void AddMessage(string message)
        {
            AddTask(message, TaskErrorCategory.Message);
        }

        private static void AddTask(string message, TaskErrorCategory category)
        {
            _errorListProvider.Tasks.Add(new ErrorTask
            {
                Category = TaskCategory.User,
                ErrorCategory = category,
                Text = message
            });
        }
    }
}