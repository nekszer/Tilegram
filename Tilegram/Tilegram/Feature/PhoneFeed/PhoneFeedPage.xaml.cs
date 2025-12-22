using System.Threading.Tasks;
using Tilegram.Feature.Feed;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tilegram.Feature.PhoneFeed
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class PhoneFeedPage : Page
    {
        private PhoneFeedViewModel ViewModel => DataContext as PhoneFeedViewModel;

        public PhoneFeedPage()
        {
            this.InitializeComponent();
        }

        #region Navegación y animaciones

        private async void NavigateToNext()
        {
            if (ViewModel == null || !ViewModel.CanLoadNext()) return;

            // Cambiar al siguiente item
            ViewModel.LoadNext();
        }

        private async void NavigateToPrevious()
        {
            if (ViewModel == null || !ViewModel.CanLoadPrevious()) return;

            // Cambiar al item anterior
            ViewModel.LoadPrevious();
        }

        #endregion

        #region Handlers de botones

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPrevious();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToNext();
        }

        #endregion

        // EVENTO: Menú de opciones (tres puntos)
        private void MoreOptions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item)
            {
                ShowOptionsFlyout(button, item);
            }
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

        // EVENTO: Ver todos los comentarios
        private void ViewComments_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item && DataContext is PhoneFeedViewModel viewModel)
            {
                // viewModel.ViewCommentsCommand.Execute(item);
            }
        }

        private void LikeButton_Checked(object sender, RoutedEventArgs e)
        {
            if(sender is ToggleButton toggleButton && toggleButton.DataContext is FeedModel item)
            {

            }
        }

        private void LikeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.DataContext is FeedModel item)
            {

            }
        }

        private void SaveButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.DataContext is FeedModel item)
            {

            }
        }

        private void SaveButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.DataContext is FeedModel item)
            {

            }
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item)
            {

            }
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item)
            {

            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null && (ViewModel.FeedItems == null || ViewModel.FeedItems.Count == 0))
            {
                await Task.Delay(1);
                ViewModel.LoadCurrentFeedItem();
                // await ViewModel.LoadFeedItemsAsync();
            }
        }

        private void EllipseImageProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel?.OpenMyProfile();
        }

        private void Caption_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is RichTextBlock caption)
            {
                if (caption.MaxLines == 2)
                {
                    caption.MaxLines = 0;
                    caption.TextWrapping = TextWrapping.Wrap;
                }
                else
                {
                    caption.MaxLines = 2;
                }
            }
        }
    }
}
