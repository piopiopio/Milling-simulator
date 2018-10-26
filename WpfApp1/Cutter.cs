using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Examples.Shapes1;
using Examples.Shapes2;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WpfApp1;

namespace ModelowanieGeometryczne.Model
{
    public class Cutter : ViewModelBase
    {

        public VBOHelpers.Vbo vbo1;
        private VBOHelpers.Vbo vbo2;
        private CutterSphere shape1;
        private CutterCylinder shape2;

        public Cutter(Vector3 centerPoint)
        {
            _centerPoint = centerPoint;
            shape1 = new CutterSphere(_centerPoint, _cutterDiameter/2);
            shape2 = new CutterCylinder(_centerPoint, _cutterDiameter/2);
        }

        public void OnLoad()
        {
            vbo1 = VBOHelpers.LoadVBO(shape1);
            vbo2 = VBOHelpers.LoadVBO(shape2);
        }


        Vector3 _centerPoint;// = new Vector3(0, 100, 0);

        public Vector3 CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                //TODO: Dodać metode do zmieniania ce 
                _centerPoint = value;
                //if (_cutterIsSphercal)
                //{
                //    shape1 = new CutterSphere(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);
                //    shape2 = new CutterCylinder(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);


                //}
                //else
                //{
                //    shape1 = new CutterSphere(CenterPoint, CutterDiameter / 2);
                //    shape2 = new CutterCylinder(CenterPoint, CutterDiameter / 2);
                //}

                shape1.CenterPoint = _centerPoint;
                shape2.CenterPoint = _centerPoint;
                //OnLoad();

                OnUpdateFrame();


                OnPropertyChanged(nameof(CenterPoint));
                Refresh();
            }
            
    }

        // private int _cutterDiameter=20;

        //public int CutterDiameter
        //{
        //    get { return _cutterDiameter; }
        //    set
        //    {
        //        _cutterDiameter = value;
        //        OnPropertyChanged(nameof(CutterDiameter));
        //    }
        //}

        private double _feedDepth;

        public double FeedDepth
        {
            get { return _feedDepth; }
            set
            {
                _feedDepth = value;
                OnPropertyChanged(nameof(FeedDepth));
            }
        }

        private bool _cutterIsSphercal = false;

        public bool CutterIsSpherical
        {
            get { return _cutterIsSphercal; }
            set
            {
                _cutterIsSphercal = value;


                if (_cutterIsSphercal)
                {
                    shape1 = new CutterSphere(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);
                    shape2 = new CutterCylinder(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);


                }
                else
                {
                    shape1 = new CutterSphere(CenterPoint, CutterDiameter / 2);
                    shape2 = new CutterCylinder(CenterPoint, CutterDiameter / 2);
                }

                OnLoad();

                OnUpdateFrame();
                OnPropertyChanged(nameof(CutterIsSpherical));
                Refresh();

            }
        }


        float _cutterDiameter = 20;

        public float CutterDiameter
        {
            get { return _cutterDiameter; }
            set
            {
                _cutterDiameter = value;
                if (_cutterIsSphercal)
                {
                    shape1 = new CutterSphere(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);
                    shape2 = new CutterCylinder(CenterPoint + new Vector3(0, CutterDiameter / 2, 0), CutterDiameter / 2);


                }
                else
                {
                    shape1 = new CutterSphere(CenterPoint, CutterDiameter / 2);
                    shape2 = new CutterCylinder(CenterPoint, CutterDiameter / 2);
                }

                OnPropertyChanged(nameof(CutterDiameter));
                Refresh();

            }
        }
        public void OnUpdateFrame()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo1.VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape1.Vertices.Length * Vector3.SizeInBytes),
                shape1.Vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo2.VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape2.Vertices.Length * Vector3.SizeInBytes),
                shape2.Vertices, BufferUsageHint.DynamicDraw);
        }



        public void Draw()
        {
            if (CutterIsSpherical)
            {
                VBOHelpers.Draw(vbo1, "QuadStrip");
            }

            VBOHelpers.Draw(vbo2, "Triangles");
            //Refresh();
        }



        public event PropertyChangedEventHandler RefreshScene;

        private void Refresh()
        {
            //BusyEllipseLed = 1;
            if (RefreshScene != null)
                RefreshScene(this, new PropertyChangedEventArgs("RefreshScene"));
            //BusyEllipseLed = 0;
        }
    }
}
