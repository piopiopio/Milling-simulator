using System.ComponentModel;

namespace ModelowanieGeometryczne.ViewModel
{
    public class ViewModelBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged!=null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

