using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelowanieGeometryczne.Helpers;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;

namespace WpfApp1
{
    public  class FileReader : ViewModelBase
    {
        ObservableCollection<LinearMillingMove> _pointsList = new ObservableCollection<LinearMillingMove>();

        public ObservableCollection<LinearMillingMove> PointsList
        {
            get { return _pointsList; }
            set
            {
                _pointsList = value;
                OnPropertyChanged(nameof(PointsList));
            }
        }

        private bool _cutterIsSpherical;

        public bool CutterIsSpherical
        {
            get { return _cutterIsSpherical; }
            set
            {
                _cutterIsSpherical = value;
                OnPropertyChanged(nameof(CutterIsSpherical));
            }
        }

        private int _cutterDiameter;

        public int CutterDiameter
        {
            get { return _cutterDiameter; }
            set
            {
                _cutterDiameter = value;
                OnPropertyChanged("CutterDiameter");
            }
        }


        public void ParseGCode(string opFileName)
        {


            var fileExtension = opFileName.Substring(opFileName.LastIndexOf('.') + 1);
            if (fileExtension.First() == 'k') CutterIsSpherical = true;
            else if (fileExtension.First() == 'f') CutterIsSpherical = false;
            else
            {
                MessageBox.Show("Unknow file type");
                return;
            }
            CutterDiameter = Int32.Parse(fileExtension.Substring(1));

            MessageBox.Show(CutterIsSpherical + "  " + CutterDiameter);
            PointsList = LoadGCode(opFileName);
        }

        public ObservableCollection<LinearMillingMove> LoadGCode(string fileName)
        {
            //public Vector3D MoveToPoint { get; set; }
            //public double LineNumber { get; set; }
            //public string LinearMoveCommandNumber { get; set; }

            ObservableCollection<LinearMillingMove> movesList = new ObservableCollection<LinearMillingMove>();
            var lines = File.ReadAllLines(fileName).ToList();

            int index = lines.IndexOf(lines.First(line => line.Contains("X") || line.Contains("Y") || line.Contains("Z")));
            string tempCommandID;
            Vector3d tempPoint = new Vector3d(0, 0, 0);

            foreach (var item in lines)
            {
                var IndexToParseLineID = item.IndexOf("G");
                var LineIDNumber = Int32.Parse(item.Substring(1, IndexToParseLineID - 1));



                var tempX = Regex.Match(item, "[X][-]?[0-9]*[.][0-9]*").Value;
                if (tempX.Length > 0)
                {
                    tempPoint.X = double.Parse(tempX.Substring(1));
                }

                var tempY = Regex.Match(item, "[Y][-]?[0-9]*[.][0-9]*").Value;
                if (tempY.Length > 0)
                {
                    tempPoint.Y = double.Parse(tempY.Substring(1));
                }

                var tempZ = Regex.Match(item, "[Z][-]?[0-9]*[.][0-9]*").Value;
                if (tempZ.Length > 0)
                {
                    tempPoint.Z = double.Parse(tempZ.Substring(1));
                }





                tempCommandID = item.Substring(IndexToParseLineID, 3);
                movesList.Add(new LinearMillingMove(tempPoint, LineIDNumber, linearMoveCommandNumber: tempCommandID));
            }


            return movesList;
        }
    }
}
