using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Tilegram.Feature.Feed
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class FeedPage : Page
    {
        private FeedViewModel ViewModel => DataContext as FeedViewModel;

        public FeedPage()
        {
            this.InitializeComponent();
        }

        // EVENTO: Scroll para carga infinita
        private void FeedListView_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var scrollViewer = GetScrollViewer(listView);

            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }
        }

        // EVENTO: Detectar cuando se llega al final para cargar más
        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            // Si está cerca del final (90%) y no está cargando ya
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight * 0.9)
            {
                if (ViewModel != null && !ViewModel.IsLoadingMore)
                {
                    // await ViewModel.LoadMoreItemsAsync();
                }
            }
        }

        // EVENTO: Clic en un post
        private void FeedListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FeedModel item && ViewModel != null)
            {
                // ViewModel.NavigateToPostDetailCommand.Execute(item);
            }
        }

        // EVENTO: Menú de opciones (tres puntos)
        private void MoreOptions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item)
            {
                ShowOptionsFlyout(button, item);
            }
        }

        // EVENTO: Ver todos los comentarios
        private void ViewComments_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item && ViewModel != null)
            {
                // ViewModel.ViewCommentsCommand.Execute(item);
            }
        }

        // EVENTO: Reproducir video
        private async void PlayVideoButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string videoUrl)
            {
                // Abrir video en reproductor nativo
                await Windows.System.Launcher.LaunchUriAsync(new Uri(videoUrl));
            }
        }

        // Helper: Obtener ScrollViewer de un ListView
        private ScrollViewer GetScrollViewer(DependencyObject element)
        {
            if (element is ScrollViewer)
                return element as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }

            return null;
        }

        // Helper: Mostrar menú de opciones
        private void ShowOptionsFlyout(FrameworkElement sender, FeedModel item)
        {
            var flyout = new MenuFlyout();

            // Reportar
            flyout.Items.Add(new MenuFlyoutItem
            {
                Text = "Report",
                Icon = new SymbolIcon(Symbol.ReportHacked),
                // Command = ViewModel?.ReportPostCommand,
                // CommandParameter = item
            });

            // Copiar enlace
            flyout.Items.Add(new MenuFlyoutItem
            {
                Text = "Copy Link",
                Icon = new SymbolIcon(Symbol.Copy),
                // Command = ViewModel?.CopyLinkCommand,
                // CommandParameter = item
            });

            // Compartir
            flyout.Items.Add(new MenuFlyoutItem
            {
                Text = "Share to...",
                Icon = new SymbolIcon(Symbol.Share),
                // Command = ViewModel?.SharePostCommand,
                // CommandParameter = item
            });

            // Guardar
            flyout.Items.Add(new MenuFlyoutItem
            {
                Text = item.HasSaved ? "Unsave" : "Save",
                Icon = new SymbolIcon(item.HasSaved ? Symbol.UnFavorite : Symbol.Favorite),
                // Command = ViewModel?.ToggleSaveCommand,
                // CommandParameter = item
            });

            /*
            // Si es nuestro post
            if (ViewModel?.CurrentUser?.Id == item.User.Id)
            {
                flyout.Items.Add(new MenuFlyoutSeparator());

                // Eliminar
                flyout.Items.Add(new MenuFlyoutItem
                {
                    Text = "Delete",
                    Icon = new SymbolIcon(Symbol.Delete),
                    Command = ViewModel?.DeletePostCommand,
                    CommandParameter = item,
                    Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 237, 73, 86))
                });
            }
            */

            flyout.ShowAt(sender);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            /*
            // Limpiar eventos para evitar memory leaks
            var scrollViewer = GetScrollViewer(FeedListView);
            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
            }
            */

            base.OnNavigatedFrom(e);
        }

        private void LikeButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LikeButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}