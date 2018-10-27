using System;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;


namespace ModelowanieGeometryczne.Helpers
{
    public class LinearMillingMove: ViewModelBase
    {
        public Vector3 _moveToPoint;
        
        

        public string X
        {
            get { return _moveToPoint.X.ToString("0.000");}
        }

        public string Y
        {
            get { return _moveToPoint.Y.ToString("0.000"); }
        }

        public string Z
        {
            get { return _moveToPoint.Z.ToString("0.000"); }
        }

        public int LineNumber { get; set; }
        public string LinearMoveCommandNumber { get; set; }


        public LinearMillingMove(Vector3 moveToPoint,int lineNumber, string linearMoveCommandNumber)
        {
            _moveToPoint = moveToPoint;
            LineNumber = lineNumber;
            LinearMoveCommandNumber = linearMoveCommandNumber;
            
        }


    }
}