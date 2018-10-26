using OpenTK.Graphics.OpenGL;

namespace ModelowanieGeometryczne.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private MillingSimulator _millingSimulator = new MillingSimulator();

        public MillingSimulator MillingSimulator1
        {
            get
            {
                return _millingSimulator;
            }
            set
            {
                _millingSimulator = value;
                OnPropertyChanged(nameof(MillingSimulator1));
                
            }
        }
    }
}

