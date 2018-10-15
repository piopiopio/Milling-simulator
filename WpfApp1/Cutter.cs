using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private CutterSphere shape1 = new CutterSphere(new Vector3(0, 100, 0), 20);
        private CutterCylinder shape2 = new CutterCylinder(new Vector3(0, 100, 0), 20);

        public Cutter()
        {

        }

        public void OnLoad()
        {
            vbo1 = VBOHelpers.LoadVBO(shape1);
            vbo2 = VBOHelpers.LoadVBO(shape2);

        }


        Vector3 _centerPoint = new Vector3();

        public Vector3 CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                _centerPoint = value;
                OnPropertyChanged(nameof(CenterPoint));
            }
        }

        private int _cutterDiameter;

        public int CutterDiameter
        {
            get { return _cutterDiameter; }
            set
            {
                _cutterDiameter = value;
                OnPropertyChanged(nameof(CutterDiameter));
            }
        }

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

        private bool _cutterIsSphercal;

        public bool CutterIsSpherical
        {
            get { return _cutterIsSphercal; }
            set
            {
                _cutterIsSphercal = value;
                OnPropertyChanged(nameof(CutterIsSpherical));
            }
        }

        float _radius;

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                Radius = _radius;

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
           VBOHelpers.Draw(vbo1, "QuadStrip");
           VBOHelpers.Draw(vbo2, "Triangles");
        }
    }
}
