using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Models
{
    // Модель фильма
    public class Film
    {
        private int _id;
        private string _name;
        private string _genre;
        private string _director;
        private int _duration;
        private string _ageRating;
        private decimal _price;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(nameof(Genre)); }
        }

        public string Director
        {
            get => _director;
            set { _director = value; OnPropertyChanged(nameof(Director)); }
        }

        public int Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        public string AgeRating
        {
            get => _ageRating;
            set { _ageRating = value; OnPropertyChanged(nameof(AgeRating)); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
