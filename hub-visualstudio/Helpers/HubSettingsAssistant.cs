using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlackDuckHub.VisualStudio.Helpers
{
    public static class HubSettingsAssistant
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if ((child != null) && child is T)
                    yield return (T) child;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        public static bool HasHubSettings(string[] hubSettings)
        {
            return !string.IsNullOrEmpty(hubSettings[0]) && !string.IsNullOrEmpty(hubSettings[1]) &&
                   !string.IsNullOrEmpty(hubSettings[2]) && !string.IsNullOrEmpty(hubSettings[3]);
        }
    }

}
