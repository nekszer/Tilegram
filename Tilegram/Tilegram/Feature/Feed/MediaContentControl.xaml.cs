using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Tilegram.Feature.Feed
{
    public sealed partial class MediaContentControl : UserControl
    {
        public static readonly DependencyProperty FeedModelProperty =
            DependencyProperty.Register("FeedModel", typeof(FeedModel),
                typeof(MediaContentControl), new PropertyMetadata(null, OnFeedModelChanged));

        private double _calculatedHeight = 400; // Valor por defecto

        public FeedModel FeedItem
        {
            get => (FeedModel)GetValue(FeedModelProperty);
            set => SetValue(FeedModelProperty, value);
        }

        public MediaContentControl()
        {
            this.InitializeComponent();
            this.Loaded += MediaContentControl_Loaded;
            this.Unloaded += MediaContentControl_Unloaded;

            // NO acceder a Window.Current aquí - puede causar el error 0xC000027B
        }

        private void MediaContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            // SOLUCIÓN SEGURA: Calcular dimensiones cuando el control está cargado
            UpdateMediaContent();
        }

        private void MediaContentControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CleanupResources();
        }

        private double GetSafeScreenWidth()
        {
            try
            {
                // Método SEGURO 1: Usar el ancho del control cuando está disponible
                if (this.ActualWidth > 0)
                    return this.ActualWidth;

                // Método SEGURO 2: Usar el ancho del contenedor
                if (MediaContainer.ActualWidth > 0)
                    return MediaContainer.ActualWidth;

                // Método SEGURO 3: Usar Window.Current SOLO si está en UI thread
                if (Window.Current != null && Window.Current.Bounds.Width > 0)
                {
                    // Verificar que estamos en el contexto correcto
                    if (Window.Current.Dispatcher.HasThreadAccess)
                    {
                        return Window.Current.Bounds.Width;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting screen width: {ex.Message}");
            }

            // Valor por defecto seguro para teléfonos
            return 360;
        }

        private void CalculateDimensions()
        {
            // Obtener ancho de forma segura
            double availableWidth = GetSafeScreenWidth();

            // Asegurar un ancho mínimo
            if (availableWidth <= 0) availableWidth = 360;

            // Restar márgenes (si los tienes en el layout)
            availableWidth -= 4; // Pequeño margen mínimo

            // Calcular altura basada en aspect ratio
            if (FeedItem != null && FeedItem.AspectRatio > 0)
            {
                _calculatedHeight = availableWidth / FeedItem.AspectRatio;

                // Límites razonables para teléfonos
                if (_calculatedHeight < 200) _calculatedHeight = 200;
                if (_calculatedHeight > 800) _calculatedHeight = 800;
            }
            else
            {
                _calculatedHeight = availableWidth; // Cuadrado por defecto
            }

            System.Diagnostics.Debug.WriteLine($"Calculated: Width={availableWidth}, Height={_calculatedHeight}");
        }

        private static void OnFeedModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MediaContentControl)d;

            // Ejecutar en el contexto de UI
            control.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                control.UpdateMediaContent();
            });
        }

        private void UpdateMediaContent()
        {
            try
            {
                MediaContainer.Children.Clear();

                if (FeedItem == null) return;

                // Calcular dimensiones
                CalculateDimensions();

                if (!FeedItem.IsVideo && !FeedItem.IsCarousel)
                {
                    // IMAGEN SIMPLE
                    var image = CreateImageWithSource(FeedItem.MediaUrl);
                    MediaContainer.Children.Add(image);
                }
                else if (FeedItem.IsVideo)
                {
                    // VIDEO
                    var mediaPlayerElement = CreateVideoPlayer(FeedItem);
                    MediaContainer.Children.Add(mediaPlayerElement);
                }
                else if (FeedItem.IsCarousel)
                {
                    try
                    {
                        // CAROUSEL
                        var flipView = CreateCarouselView(FeedItem);
                        MediaContainer.Children.Add(flipView);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating carousel: {ex.Message}");
                        // Fallback a imagen simple
                        var image = CreateImageWithSource(FeedItem.MediaUrl);
                        MediaContainer.Children.Add(image);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR in UpdateMediaContent: {ex.Message}");
                // Mostrar contenido de error
                ShowErrorContent();
            }
        }

        private Image CreateImageWithSource(string imageUrl)
        {
            var image = new Image
            {
                Stretch = Stretch.UniformToFill,
                Height = _calculatedHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };

            // Configurar la imagen de forma asíncrona
            ConfigureImageAsync(image, imageUrl);

            return image;
        }

        private async void ConfigureImageAsync(Image image, string imageUrl)
        {
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.ImageOpened += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Image loaded: {imageUrl}");
                };

                bitmapImage.ImageFailed += (s, e) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Image failed: {imageUrl}, Error: {e.ErrorMessage}");
                };

                bitmapImage.UriSource = new Uri(imageUrl);
                bitmapImage.CreateOptions = BitmapCreateOptions.None;

                image.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error configuring image: {ex.Message}");
            }
        }

        private MediaPlayerElement CreateVideoPlayer(FeedModel feedItem)
        {
            var mediaPlayerElement = new MediaPlayerElement
            {
                Height = _calculatedHeight,
                AreTransportControlsEnabled = true,
                AutoPlay = false,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };

            try
            {
                if (!string.IsNullOrEmpty(feedItem.MediaUrl))
                {
                    mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(feedItem.MediaUrl));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating video source: {ex.Message}");

                // Fallback a thumbnail
                var fallbackGrid = new Grid();
                var fallbackImage = new Image
                {
                    Source = new BitmapImage(new Uri(feedItem.ThumbnailUrl ?? feedItem.MediaUrl)),
                    Stretch = Stretch.UniformToFill,
                    Height = _calculatedHeight
                };
                fallbackGrid.Children.Add(fallbackImage);

                // No devolver MediaPlayerElement si falló, devolver Grid con imagen
                return new MediaPlayerElement(); // O manejar de otra forma
            }

            return mediaPlayerElement;
        }

        private FlipView CreateCarouselView(FeedModel feedItem)
        {
            var flipView = new FlipView
            {
                Height = _calculatedHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };

            if (feedItem.CarouselUrls != null)
            {
                foreach (var url in feedItem.CarouselUrls)
                {
                    var image = CreateImageWithSource(url);
                    flipView.Items.Add(image);
                }
            }

            return flipView;
        }

        private void CleanupResources()
        {
            try
            {
                foreach (var child in MediaContainer.Children)
                {
                    if (child is MediaPlayerElement mediaPlayer)
                    {
                        try
                        {
                            mediaPlayer.MediaPlayer?.Pause();
                            mediaPlayer.Source = null;
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private void ShowErrorContent()
        {
            MediaContainer.Children.Clear();

            var errorText = new TextBlock
            {
                Text = "Unable to load media",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Windows.UI.Colors.Gray)
            };

            MediaContainer.Children.Add(errorText);
        }
    }
}