using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        // AHORA esta colección será para el Bento Grid
        private ObservableCollection<Post> _posts = new ObservableCollection<Post>();
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InstagramProfileService.InstagramProfileHighlight> _highlights = new ObservableCollection<InstagramProfileService.InstagramProfileHighlight>();
        public ObservableCollection<InstagramProfileService.InstagramProfileHighlight> Highlights
        {
            get => _highlights;
            set
            {
                _highlights = value;
                OnPropertyChanged();
            }
        }

        // Propiedad adicional para controlar el layout (opcional)
        private int _gridColumns = 3;
        public int GridColumns
        {
            get => _gridColumns;
            set
            {
                _gridColumns = value;
                OnPropertyChanged();
            }
        }

        public ProfileViewModel()
        {
            LoadDemoProfile();

            LoadProfileData();
        }

        private void LoadDemoProfile()
        {
            Profile = new ProfileData
            {
                UserName = "aliensofttech",
                PostCount = 1,
                Picture = "/Assets/Demo/ast.png",
                FullName = "Alien Soft Tech",
                Description = "WinPhone 10 📱 Software Developer",
                FollowerCount = 331493,
                FollowingCount = 93
            };
        }

        private async void LoadHighlights(InstagramProfileService service)
        {
            var highlights = await service.LoadHightlights();
            foreach (var item in highlights)
                Highlights.Add(item);
        }

        private async void LoadProfileData()
        {
            // Cargar posts con Bento Grid
            await LoadPostsFromService();

            // Cargar perfil
            await LoadProfile();
        }

        // Método para cargar posts desde un servicio (ejemplo)
        public async Task LoadPostsFromService()
        {
            var service = Light.UWP.Services.IoC.Container.Instance.Resolve<ProfileService>();
            var mePostsEither = await service.MePosts();

            mePostsEither.Match(ex =>
            {

            }, success =>
            {
                foreach (var post in success.Posts)
                    Posts.Add(Post.CreateWithRandomSize(post.Images.FirstOrDefault(), post.Text ?? string.Empty, post.LikesCount, post.TakenAt));
            });
        }

        #region Load Profile
        private async Task LoadProfile()
        {
            var service = Light.UWP.Services.IoC.Container.Instance.Resolve<ProfileService>();
            if (service == null)
                return;

            var meEither = await service.Me();
            meEither.Match(OnMeError, OnMeSuccess);
        }

        private void OnMeError(Exception obj)
        {
            // Manejar error
            System.Diagnostics.Debug.WriteLine($"Error loading profile: {obj.Message}");
        }

        private void OnMeSuccess(ProfileData profileData)
        {
            Profile = profileData;
        }
        #endregion

        #region Observable
        public event PropertyChangedEventHandler PropertyChanged;

        internal void ChangePostSize(int index, int size)
        {
            Posts[index].GridSize = size;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
