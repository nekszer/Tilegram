using Light.UWP.Services.Navigation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tilegram.Feature.Feed;
using Tilegram.Services.Feed;

namespace Tilegram.Feature.PhoneFeed
{
    public class PhoneFeedViewModel : INotifyPropertyChanged
    {
        #region FeedItems
        private ObservableCollection<FeedModel> _feedItems;
        public ObservableCollection<FeedModel> FeedItems
        {
            get => _feedItems;
            set
            {
                _feedItems = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CurrentFeedItem
        private FeedModel _currentFeedItem;
        public FeedModel CurrentFeedItem
        {
            get => _currentFeedItem;
            set
            {
                _currentFeedItem = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CurrentIndex
        private int _currentIndex;
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                if (FeedItems != null && FeedItems.Count > value)
                {
                    CurrentFeedItem = FeedItems[value];
                }
                OnPropertyChanged();
            }
        }
        #endregion

        public void LoadNext()
        {
            if (FeedItems != null && CurrentIndex < FeedItems.Count - 1)
            {
                CurrentIndex++;
            }
        }

        public void LoadPrevious()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }
        }

        public void LoadCurrentFeedItem()
        {
            FeedItems = new ObservableCollection<FeedModel>();

            FeedItems.Add(new FeedModel
            {
                IsCarousel = true,
                MediaType = 8,

                CarouselUrls = new List<string>
                {
                    "https://lipsum.app/random/800x1000",
                    "https://lipsum.app/random/800x1000",
                    "https://lipsum.app/random/800x1000",
                    "https://lipsum.app/random/800x1000"
                },

                MediaUrl = "https://lipsum.app/random/800x1000",
                ThumbnailUrl = "https://lipsum.app/random/800x1000",

                Width = 800,
                Height = 1000,
                AspectRatio = 0.8,

                User = new Feed.User
                {
                    Username = "local_test",
                    FullName = "Local Development",
                    ProfilePictureUrl = "https://lipsum.app/random/300x300"
                },

                Caption = "Testing carousel with local images 📱 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                LikesCount = 999,
                CommentsCount = 42,
                LikesCountFormatted = "999",
                CommentsCountFormatted = "42",
                TimeAgo = "Just now"
            });

            FeedItems.Add(new FeedModel
            {
                IsCarousel = true,
                MediaType = 8,

                CarouselUrls = new List<string>
                {
                    "https://lipsum.app/id/24/800x1000",
                    "https://lipsum.app/id/25/800x1000"
                },

                MediaUrl = "https://lipsum.app/id/27/800x1000",
                ThumbnailUrl = "https://lipsum.app/id/27/800x1000",

                Width = 800,
                Height = 1000,
                AspectRatio = 0.8,

                User = new Feed.User
                {
                    Username = "local_test",
                    FullName = "Local Development",
                    ProfilePictureUrl = "https://lipsum.app/id/1/300x300"
                },

                Caption = "Testing carousel with local images 📱 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                LikesCount = 999,
                CommentsCount = 42,
                LikesCountFormatted = "999",
                CommentsCountFormatted = "42",
                TimeAgo = "Just now"
            });

            FeedItems.Add(new FeedModel
            {
                IsCarousel = true,
                MediaType = 8,

                CarouselUrls = new List<string>
                {
                    "https://lipsum.app/id/1/800x1000",
                    "https://lipsum.app/id/2/800x1000",
                    "https://lipsum.app/id/3/800x1000"
                },

                MediaUrl = "https://lipsum.app/id/4/800x1000",
                ThumbnailUrl = "https://lipsum.app/id/4/800x1000",

                Width = 800,
                Height = 1000,
                AspectRatio = 0.8,

                User = new Feed.User
                {
                    Username = "local_test",
                    FullName = "Local Development",
                    ProfilePictureUrl = "https://lipsum.app/id/4/300x300"
                },

                Caption = "Testing carousel with local images 📱 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                LikesCount = 999,
                CommentsCount = 42,
                LikesCountFormatted = "999",
                CommentsCountFormatted = "42",
                TimeAgo = "Just now"
            });

            if (FeedItems.Count > 0)
                CurrentIndex = 0;
        }

        public void OpenMyProfile()
        {
            NavigationService.Instance.NavigateTo(AppRoutes.Profile);
        }

        // Cargar datos desde API
        public async Task LoadFeedItemsAsync()
        {
            await Task.Delay(1);
            LoadCurrentFeedItem();
            // Tu lógica para cargar desde la API de Instagram
            //var feedService = Light.UWP.Services.IoC.Container.Instance.Resolve<FeedService>();
            //if (feedService == null)
            //    return;

            //var userFeedEither = await feedService.UserFeed();
            //userFeedEither.Match(ex =>
            //{

            //}, items =>
            //{
            //    FeedItems = new ObservableCollection<FeedModel>();
            //    foreach (var item in items)
            //        FeedItems.Add(GetFeedItem(item));

            //    if (FeedItems.Count > 0)
            //        CurrentIndex = 0;
            //});
        }

        private FeedModel GetFeedItem(FeedItem item)
        {
            var json = JsonConvert.SerializeObject(item);
            return JsonConvert.DeserializeObject<FeedModel>(json);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanLoadNext()
        {
            return FeedItems != null && CurrentIndex < FeedItems.Count - 1;
        }

        public bool CanLoadPrevious()
        {
            return CurrentIndex > 0;
        }

        // Métodos para carga progresiva (si necesitas cargar más datos al llegar al final)
        public async Task LoadMoreItemsIfNeeded()
        {
            if (CurrentIndex >= FeedItems.Count - 3) // Cargar más cuando queden 3 elementos
            {
                // await LoadMoreFeedItemsAsync();
            }
        }

        //private async Task LoadMoreFeedItemsAsync()
        //{
        //    // Tu lógica para cargar más elementos de la API
        //    var newItems = await InstagramApiService.GetMoreFeedItems();
        //    foreach (var item in newItems)
        //    {
        //        FeedItems.Add(item);
        //    }
        //}
    }
}
