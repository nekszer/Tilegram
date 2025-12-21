using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tilegram.Feature.Profile
{
    public class Post : INotifyPropertyChanged
    {
        private string _imagePath;
        private string _title;
        private int _gridSize = 1; // Por defecto tamaño pequeño
        private long _likes;
        private DateTime _date;
        private string _dateStr;

        public string DateStr
        {
            get => _dateStr;
            set
            {
                _dateStr = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        // Tamaño del item en el Bento Grid: 1=pequeño (1x1), 2=mediano (2x2), 3=grande (3x3)
        public int GridSize
        {
            get => _gridSize;
            set
            {
                _gridSize = value;
                OnPropertyChanged();
                // Notificar cambios en propiedades calculadas
                OnPropertyChanged(nameof(ColumnSpan));
                OnPropertyChanged(nameof(RowSpan));
            }
        }

        public long Likes
        {
            get => _likes;
            set
            {
                _likes = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                DateStr = value.ToString("dd/MM/yy");
                OnPropertyChanged();
            }
        }

        // Propiedades calculadas para el Bento Grid
        public int ColumnSpan => GridSize;
        public int RowSpan => GridSize;

        // Método para alternar el tamaño (opcional)
        public void ToggleSize()
        {
            GridSize = GridSize == 1 ? 2 : 1;
        }

        // Método estático para crear posts con tamaño aleatorio
        public static Post CreateWithRandomSize(string imagePath, string title, long likes, DateTime date)
        {
            var random = new Random();
            // 60% pequeños, 30% medianos, 10% grandes
            var randomValue = random.Next(100);
            int gridSize;

            if (randomValue < 60)
                gridSize = 1; // Pequeño
            else if (randomValue < 90)
                gridSize = 2; // Mediano
            else
                gridSize = 3; // Grande

            return new Post
            {
                ImagePath = imagePath,
                Title = title,
                GridSize = gridSize,
                Likes = likes,
                Date = date
            };
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
