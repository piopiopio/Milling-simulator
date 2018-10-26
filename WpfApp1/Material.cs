using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examples.Shapes;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WpfApp1
{
    public class Material : ViewModelBase
    {
        public Material()
        {
            _materialWidth=150;
            _materialHeight = 50;
            _materialDepth=150;
            _divisions = 100;
            _shape0 = new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
        }
        private Cubes _shape0;

        public Cubes Shape0
        {
            get { return _shape0; }
            set
            {
                _shape0 = value;
                OnPropertyChanged(nameof(Shape0));
            }
        }

        public VBOHelpers.Vbo vbo;
        public void OnLoad()
        {
            vbo = VBOHelpers.LoadVBO(Shape0);

        }
        private double[,] _heightArray;


        public void OnUpdateFrame()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Shape0.Vertices.Length * Vector3.SizeInBytes),
                Shape0.Vertices, BufferUsageHint.DynamicDraw);
        }

        private double _materialWidth;
        public double MaterialWidth
        {
            get { return _materialWidth; }
            set
            {
                _materialWidth = value;
                _shape0=new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
                OnLoad();
                OnPropertyChanged(nameof(MaterialWidth));
            }
        }


        private double _materialHeight;
        public double MaterialHeight
        {
            get { return _materialHeight; }
            set
            {
                _materialHeight = value;
                _shape0 = new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
                OnLoad();
                OnPropertyChanged(nameof(MaterialHeight));
            }
        }



        private double _materialDepth;
        public double MaterialDepth
        {
            get { return _materialDepth; }
            set
            {
                _materialDepth = value;
                _shape0 = new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
                OnLoad();
                OnPropertyChanged(nameof(MaterialDepth));
            }
        }


        public int _divisions;

        public int Divisions
        {
            get { return _divisions; }
            set
            {
                _divisions = value;
                _shape0 = new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
                OnLoad();
                OnPropertyChanged(nameof(Divisions));
            }
        }

        public void Draw()
        {
            VBOHelpers.Draw(vbo, "Quad");

        }
    }
}
