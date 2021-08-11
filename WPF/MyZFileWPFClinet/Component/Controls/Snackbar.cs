using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace Component
{
    public enum SnackbarActionButtonPlacementMode
    {
        Auto,
        Inline,
        SeparateLine
    }

    //[ContentProperty(nameof(Message))]
    public class Snackbar : Control
    {
        private const string ActivateStoryboardName = "ActivateStoryboard";
        private const string DeactivateStoryboardName = "DeactivateStoryboard";
    }
}
