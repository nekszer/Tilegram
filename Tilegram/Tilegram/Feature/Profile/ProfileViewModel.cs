using Light.UWP.Services.IoC;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tilegram.Services.Profile;

namespace Tilegram.Feature.Profile
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private ProfileData _profile;
        public ProfileData Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InstagramProfileService.InstagramProfileHighlight> _highlights;
        public ObservableCollection<InstagramProfileService.InstagramProfileHighlight> Highlights
        {
            get => _highlights;
            set
            {
                _highlights = value;
                OnPropertyChanged();
            }
        }

        public ProfileViewModel()
        {
            Posts = new ObservableCollection<Post>();
            Highlights = new ObservableCollection<InstagramProfileService.InstagramProfileHighlight>();

            LoadProfileData();
            // LoadHighlights(service);
        }

        private async void LoadHighlights(InstagramProfileService service)
        {
            var hightLights = await service.LoadHightlights();
            // Simulación: en producción deserializas el JSON
            foreach (var item in hightLights)
                Highlights.Add(item);
        }

        private async void LoadProfileData()
        {
            // Simulación de publicaciones (ejemplo)
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/1/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/2/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/3/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/4/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/5/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/6/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/7/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/8/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/9/200x200" });
            Posts.Add(new Post { ImagePath = "https://lipsum.app/id/10/200x200" });

            var service = Container.Instance.Resolve<ProfileService>();
            if (service == null)
                return;

            var meEither = await service.Me();
            meEither.Match(OnMeError, OnMeSuccess);
        }

        private void OnMeError(Exception obj)
        {
            // TODO: No podemos cargar tu perfil
        }

        private void OnMeSuccess(ProfileData profileData)
        {
            Profile = profileData;
        }

        #region Observable
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    // Clase auxiliar para publicaciones
    public class Post
    {
        public string ImagePath { get; set; }
    }

}
