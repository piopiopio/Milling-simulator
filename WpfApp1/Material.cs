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

        private Shape shape = new Cubes();

        public VBOHelpers.Vbo vbo;
        public void OnLoad()
        {
            vbo = VBOHelpers.LoadVBO(shape);

        }
        private double[,] _heightArray;


        private double _materialWidth = 150;
        public double MaterialWidth
        {
            get { return _materialWidth; }
            set
            {
                _materialWidth = value;
                OnPropertyChanged(nameof(MaterialWidth));
            }
        }


        private double _materialHeight = 50;
        public double MaterialHeight
        {
            get { return _materialHeight; }
            set
            {
                _materialHeight = value;
                OnPropertyChanged(nameof(MaterialHeight));
            }
        }



        private double _materialDepth = 150;
        public double MaterialDepth
        {
            get { return _materialDepth; }
            set
            {
                _materialDepth = value;
                OnPropertyChanged(nameof(MaterialDepth));
            }
        }

        public void OnUpdateFrame()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(shape.Vertices.Length * Vector3.SizeInBytes),
                shape.Vertices, BufferUsageHint.DynamicDraw);
        }


        public void Draw()
        {
            VBOHelpers.Draw(vbo, "Quad");

        }
    }
}
