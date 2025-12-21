using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tilegram.Feature.Profile
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {


        public ProfilePage()
        {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private Post _selectedPostForContext;

        private void OnPostItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Post selectedPost)
            {
                // Guardar para uso en menús contextuales
                _selectedPostForContext = selectedPost;

                // Navegar o mostrar detalle
                ShowPostDetail(selectedPost);
            }
        }

        private async void ShowPostDetail(Post post)
        {
            // Dialogo simple para UWP 15063
            var dialog = new ContentDialog
            {
                Title = post.Title ?? "Publicación",
                Content = new StackPanel
                {
                    Children =
                    {
                        new Image
                        {
                            Source = new BitmapImage(new Uri(post.ImagePath)),
                            Height = 200,
                            Stretch = Stretch.Uniform
                        },
                        new TextBlock
                        {
                            Text = $"❤️ {post.Likes} likes",
                            Margin = new Thickness(0, 12, 0, 0),
                            FontSize = 16
                        },
                        new TextBlock
                        {
                            Text = post.Title,
                            FontSize = 14,
                            Foreground = new SolidColorBrush(Colors.Gray)
                        }
                    }
                },
                IsPrimaryButtonEnabled = false,
                CloseButtonText = "Cerrar"
            };

            await dialog.ShowAsync();
        }

        private void OnPostOptionsClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Post post)
            {
                _selectedPostForContext = post;

                // Menú flyout manual
                var flyout = new MenuFlyout();
                var shareItem = new MenuFlyoutItem
                {
                    Text = "Compartir",
                    Icon = new SymbolIcon(Symbol.Share)
                };

                var deleteItem = new MenuFlyoutItem
                {
                    Text = "Eliminar",
                    Icon = new SymbolIcon(Symbol.Delete),
                    Foreground = new SolidColorBrush(Colors.Red)
                };
                deleteItem.Click += OnDeletePostClick;

                flyout.Items.Add(shareItem);
                flyout.Items.Add(new MenuFlyoutSeparator());
                flyout.Items.Add(deleteItem);

                flyout.ShowAt(button);
            }
        }

        private void OnChangePostSizeClick(object sender, RoutedEventArgs e)
        {
            if (_selectedPostForContext != null && DataContext is ProfileViewModel viewModel)
            {
                // Encontrar el índice del post seleccionado
                var index = viewModel.Posts.IndexOf(_selectedPostForContext);
                if (index >= 0)
                {
                    // Alternar entre tamaño 1 y 2
                    viewModel.ChangePostSize(index, _selectedPostForContext.GridSize == 1 ? 2 : 1);
                }
            }
        }

        private async void OnDeletePostClick(object sender, RoutedEventArgs e)
        {
            if (_selectedPostForContext != null && DataContext is ProfileViewModel viewModel)
            {
                var dialog = new ContentDialog
                {
                    Title = "Eliminar publicación",
                    Content = "¿Estás seguro de que quieres eliminar esta publicación?",
                    PrimaryButtonText = "Eliminar",
                    SecondaryButtonText = "Cancelar"
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    viewModel.Posts.Remove(_selectedPostForContext);
                }
            }
        }

        private void OnBentoGridLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is GridView gridView)
            {
                // Ajustar diseño responsivo
                UpdateGridLayout(gridView);
            }
        }

        private void OnBentoGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is GridView gridView)
            {
                UpdateGridLayout(gridView);
            }
        }

        private void UpdateGridLayout(GridView gridView)
        {
            // Ajustar número de columnas según ancho disponible
            var actualWidth = gridView.ActualWidth;
            var panel = gridView.ItemsPanelRoot as VariableSizedWrapGrid;

            if (panel != null)
            {
                if (actualWidth > 600)
                {
                    panel.MaximumRowsOrColumns = 4;
                    panel.ItemWidth = 120;
                    panel.ItemHeight = 120;
                }
                else if (actualWidth > 400)
                {
                    panel.MaximumRowsOrColumns = 3;
                    panel.ItemWidth = 140;
                    panel.ItemHeight = 140;
                }
                else
                {
                    panel.MaximumRowsOrColumns = 2;
                    panel.ItemWidth = 160;
                    panel.ItemHeight = 160;
                }
            }
        }

        private async void OnAddFirstPostClick(object sender, RoutedEventArgs e)
        {
            await AddNewPost();
        }

        private async void OnAddNewPostClick(object sender, RoutedEventArgs e)
        {
            await AddNewPost();
        }

        private async Task AddNewPost()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Aquí deberías procesar y guardar la imagen
                // Por ahora usamos una URL de ejemplo
                if (DataContext is ProfileViewModel viewModel)
                {
                    // Crear post con tamaño aleatorio
                    var random = new Random();
                    var size = random.Next(100) < 70 ? 1 : 2; // 70% pequeños, 30% grandes

                    var newPost = new Post
                    {
                        ImagePath = $"https://lipsum.app/id/{viewModel.Posts.Count + 100}/400x400",
                        Title = $"Nueva foto #{viewModel.Posts.Count + 1}",
                        GridSize = size,
                        Likes = random.Next(10, 100),
                        Date = DateTime.Now
                    };

                    viewModel.Posts.Insert(0, newPost);
                }
            }
        }
    }
}
