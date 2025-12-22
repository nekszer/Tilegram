using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Light.UWP.MVVM.View
{
    public static class CommandExtensions
    {
        // Para eventos Tapped
        public static readonly DependencyProperty TappedCommandProperty =
            DependencyProperty.RegisterAttached("TappedCommand", typeof(ICommand),
                typeof(CommandExtensions), new PropertyMetadata(null, OnTappedCommandChanged));

        public static ICommand GetTappedCommand(DependencyObject obj)
            => (ICommand)obj.GetValue(TappedCommandProperty);

        public static void SetTappedCommand(DependencyObject obj, ICommand value)
            => obj.SetValue(TappedCommandProperty, value);

        private static void OnTappedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                element.Tapped -= OnTapped;

                if (e.NewValue is ICommand command)
                {
                    element.Tapped += OnTapped;
                }
            }
        }

        private static void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                var command = GetTappedCommand(element);
                var parameter = GetTappedCommandParameter(element);

                if (command?.CanExecute(parameter) == true)
                {
                    command.Execute(parameter);
                    e.Handled = true;
                }
            }
        }

        // Para el parámetro del comando
        public static readonly DependencyProperty TappedCommandParameterProperty =
            DependencyProperty.RegisterAttached("TappedCommandParameter", typeof(object),
                typeof(CommandExtensions), new PropertyMetadata(null));

        public static object GetTappedCommandParameter(DependencyObject obj)
            => obj.GetValue(TappedCommandParameterProperty);

        public static void SetTappedCommandParameter(DependencyObject obj, object value)
            => obj.SetValue(TappedCommandParameterProperty, value);
    }
}
