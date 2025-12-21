using Light.UWP.Services.IoC;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Tilegram.Services.Feed;
using Windows.UI.Popups;

namespace Tilegram.Feature.Feed
{
    public class FeedViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<FeedModel> _feedItems;
        public ObservableCollection<FeedModel> FeedItems
        {
            get => _feedItems;
            set => SetProperty(ref _feedItems, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isLoadingMore;
        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set => SetProperty(ref _isLoadingMore, value);
        }

        public FeedViewModel()
        {
            FeedItems = new ObservableCollection<FeedModel>();
            LoadFeed();
        }

        private async void LoadFeed()
        {
            IsLoading = true;
            try
            {
                // Cargar datos del API
                var feedService = Container.Instance.Resolve<FeedService>();
                var feedData = await feedService.UserFeed();
                feedData.Match(OnUserFeedError, success =>
                {
                    // foreach (var item in success)
                        FeedItems.Add(GetFeedItem(success.FirstOrDefault()));
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void OnUserFeedError(Exception exception)
        {
            MessageDialog messageDialog = new MessageDialog("Info");
            messageDialog.Content = "Ocurrio un error al extraer el feed del usuario";
            await messageDialog.ShowAsync();
        }

        private FeedModel GetFeedItem(FeedItem item)
        {
            var json = JsonConvert.SerializeObject(item);
            return JsonConvert.DeserializeObject<FeedModel>(json);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return;

            storage = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
