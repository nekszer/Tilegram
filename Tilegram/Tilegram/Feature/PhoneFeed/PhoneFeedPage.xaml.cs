using System;
using System.Threading.Tasks;
using Tilegram.Feature.Feed;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tilegram.Feature.PhoneFeed
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class PhoneFeedPage : Page
    {
        private PhoneFeedViewModel ViewModel => DataContext as PhoneFeedViewModel;
        private double _initialX;
        private double _currentX;
        private const double SwipeThreshold = 100;
        private bool _isSwiping = false;

        public PhoneFeedPage()
        {
            this.InitializeComponent();

            // Cargar datos cuando se navega a la página
            this.Loaded += async (s, e) =>
            {
                if (ViewModel != null && (ViewModel.FeedItems == null || ViewModel.FeedItems.Count == 0))
                {
                    await Task.Delay(1);
                    ViewModel.LoadCurrentFeedItem();
                    // await ViewModel.LoadFeedItemsAsync();
                }
            };
        }

        #region Manipulation Events (Swipe)

        private void PostContainer_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initialX = e.Position.X;
            _currentX = 0;
            _isSwiping = true;

            // Inicializar transformaciones
            //ImageTransform.TranslateX = 0;
            //OverlayTransform.X = 0;
            //SwipeOverlay.Opacity = 0;
        }

        private void PostContainer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!_isSwiping) return;

            _currentX = e.Cumulative.Translation.X;

            // Aplicar transformación a la imagen
            // ImageTransform.TranslateX = _currentX;

            // Mostrar overlay para indicar dirección
            if (Math.Abs(_currentX) > 10)
            {
                //SwipeOverlay.Opacity = Math.Min(Math.Abs(_currentX) / 200, 0.4);
                //OverlayTransform.X = _currentX > 0 ? 0 : ActualWidth;

                // Cambiar color del overlay según dirección
                var color = _currentX > 0 ? "#4000FF00" : "#40FF0000";
                //SwipeOverlay.Fill = (SolidColorBrush)Application.Current.Resources[color] ?? new SolidColorBrush(_currentX > 0 ? Windows.UI.Color.FromArgb(64, 0, 255, 0) : Windows.UI.Color.FromArgb(64, 255, 0, 0));
            }

            e.Handled = true;
        }

        private void PostContainer_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!_isSwiping) return;

            var finalX = e.Cumulative.Translation.X;

            // Determinar si se completó un swipe
            if (Math.Abs(finalX) > SwipeThreshold)
            {
                if (finalX > 0)
                {
                    // Swipe a la derecha - ir al anterior
                    NavigateToPrevious();
                }
                else
                {
                    // Swipe a la izquierda - ir al siguiente
                    NavigateToNext();
                }
            }
            else
            {
                // Resetear posición si no se completó el swipe
                ResetPosition();
            }

            _isSwiping = false;
            e.Handled = true;
        }

        #endregion

        #region Pointer Events para áreas táctiles

        private DateTime _leftTapTime;
        private void LeftArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _leftTapTime = DateTime.Now;

            // Mostrar flecha izquierda
            var fadeIn = new DoubleAnimation
            {
                To = 0.5,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(fadeIn, LeftArrow);
            Storyboard.SetTargetProperty(fadeIn, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            storyboard.Begin();
        }

        private void LeftArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var tapDuration = (DateTime.Now - _leftTapTime).TotalMilliseconds;

            // Ocultar flecha
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(fadeOut, LeftArrow);
            Storyboard.SetTargetProperty(fadeOut, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeOut);
            storyboard.Begin();

            // Si fue un tap corto, navegar
            if (tapDuration < 500)
            {
                NavigateToPrevious();
            }
        }

        private DateTime _rightTapTime;
        private void RightArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _rightTapTime = DateTime.Now;

            // Mostrar flecha derecha
            var fadeIn = new DoubleAnimation
            {
                To = 0.5,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(fadeIn, RightArrow);
            Storyboard.SetTargetProperty(fadeIn, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            storyboard.Begin();
        }

        private void RightArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var tapDuration = (DateTime.Now - _rightTapTime).TotalMilliseconds;

            // Ocultar flecha
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(fadeOut, RightArrow);
            Storyboard.SetTargetProperty(fadeOut, "Opacity");

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeOut);
            storyboard.Begin();

            // Si fue un tap corto, navegar
            if (tapDuration < 500)
            {
                NavigateToNext();
            }
        }

        #endregion

        #region Navegación y animaciones

        private async void NavigateToNext()
        {
            if (ViewModel == null || !ViewModel.CanLoadNext()) return;

            // Animación de salida
            await AnimateTransition(-ActualWidth, 0);

            // Cambiar al siguiente item
            ViewModel.LoadNext();

            // Animación de entrada
            await AnimateTransition(ActualWidth, 0);
        }

        private async void NavigateToPrevious()
        {
            if (ViewModel == null || !ViewModel.CanLoadPrevious()) return;

            // Animación de salida
            await AnimateTransition(ActualWidth, 0);

            // Cambiar al item anterior
            ViewModel.LoadPrevious();

            // Animación de entrada
            await AnimateTransition(-ActualWidth, 0);
        }

        private async System.Threading.Tasks.Task AnimateTransition(double from, double to)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            //var storyboard = new Storyboard();
            //// Storyboard.SetTarget(animation, ImageTransform);
            //// Storyboard.SetTargetProperty(animation, "TranslateX");

            //storyboard.Children.Add(animation);

            //var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
            //storyboard.Completed += (s, e) => tcs.SetResult(true);

            //storyboard.Begin();
            // await tcs.Task;
        }

        private void ResetPosition()
        {
            //var animation = new DoubleAnimation
            //{
            //    To = 0,
            //    Duration = TimeSpan.FromMilliseconds(200),
            //    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            //};

            //var storyboard = new Storyboard();
            //// Storyboard.SetTarget(animation, ImageTransform);
            //Storyboard.SetTargetProperty(animation, "TranslateX");

            //// También animar el overlay
            //var overlayAnimation = new DoubleAnimation
            //{
            //    To = 0,
            //    Duration = TimeSpan.FromMilliseconds(200)
            //};
            //// Storyboard.SetTarget(overlayAnimation, SwipeOverlay);
            //Storyboard.SetTargetProperty(overlayAnimation, "Opacity");

            //storyboard.Children.Add(animation);
            //storyboard.Children.Add(overlayAnimation);
            //storyboard.Begin();
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

        #region Navegación con teclado

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                    NavigateToPrevious();
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.Space:
                    NavigateToNext();
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.Home:
                    if (ViewModel != null)
                        ViewModel.CurrentIndex = 0;
                    e.Handled = true;
                    break;

                case Windows.System.VirtualKey.End:
                    if (ViewModel != null && ViewModel.FeedItems != null)
                        ViewModel.CurrentIndex = ViewModel.FeedItems.Count - 1;
                    e.Handled = true;
                    break;
            }
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

        // EVENTO: Ver todos los comentarios
        private void ViewComments_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FeedModel item && DataContext != null)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
