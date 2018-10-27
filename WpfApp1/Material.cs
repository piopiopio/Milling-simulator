﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Shell;
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
            _materialWidth = 150;
            _materialHeight = 50;
            _materialDepth = 150;
            _divisions = 200;
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
        //private double[,] _heightArray;


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
                _shape0 = new Cubes(_materialWidth, _materialHeight, _materialDepth, _divisions);
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

        public void Cut(Vector3 startPoint, Vector3 endPoint, double diameter, bool isSpherical)
        {
            //TODO: zrobić dla roznych wartosci materialwidth, materialdepth i divisions teraz wszystkie musza byc takie same!!!   
            //var singleCubeWidth = _materialWidth / _divisions;
            //var singleCubeDepth = _materialDepth / _divisions;

            // Shape0.heightArray[(int)Math.Round(startPoint.X+_divisions/2,0), (int)Math.Round(-startPoint.Z+_divisions / 2, 0)] = startPoint.Y;


            //   OnLoad();

            //Circle(
            //        (int)Math.Round(startPoint.X + _divisions / 2, 0),
            //        (int)Math.Round(-startPoint.Z + _divisions / 2, 0),
            //        (float)Math.Round(startPoint.Y, 0),
            //        (int)diameter / 2,
            //    isSpherical);

            //Circle(
            //    (int)Math.Round(endPoint.X + _divisions / 2, 0),
            //    (int)Math.Round(-endPoint.Z + _divisions / 2, 0),
            //    (float)Math.Round(endPoint.Y, 0),
            //    (int)diameter / 2,
            //    isSpherical);


            Circle(
                (int)Math.Round(startPoint.X + _divisions / 2, 0),
                (int)Math.Round(startPoint.Y + _divisions / 2, 0),
                (float)Math.Round(startPoint.Z, 0),
                (int)diameter / 2,
                isSpherical);

            Circle(
                (int)Math.Round(endPoint.X + _divisions / 2, 0),
                (int)Math.Round(endPoint.Y + _divisions / 2, 0),
                (float)Math.Round(endPoint.Z, 0),
                (int)diameter / 2,
                isSpherical);


            //Circle((int) Math.Round(endPoint.X + _divisions / 2, 0), (int)Math.Round(-endPoint.Z + _divisions / 2, 0), (int)diameter/2);

            // 
            Fullfilment(startPoint, endPoint, diameter, isSpherical);

            //foreach (var point in Line)
            //{
            //    if (Shape0.heightArray.GetLength(0) > point.Item1 && Shape0.heightArray.GetLength(1) > point.Item2 && point.Item1 >= 0 && point.Item2 >= 0) Shape0.heightArray[point.Item1, point.Item2] =100;
            //}

            Shape0.RefreshVertices();
            OnUpdateFrame();

        }
        List<Tuple<int, int>> Line = new List<Tuple<int, int>>();

        private void Fullfilment(Vector3 startPoint, Vector3 endPoint, double diameter, bool isSpherical)
        {
            double radius = diameter / 2;
            Vector3 direction = endPoint - startPoint;
            var by = (float)Math.Sqrt(radius * radius * direction.X * direction.X /
                                       (direction.X * direction.X + direction.Y * direction.Y));
            float bx;
            if (direction.X != 0)
            {
                bx = -direction.Y * by / direction.X;
            }
            else if (direction.Y != 0)
            {
                bx = (float)radius;
            }
            else
            {
                bx = 0;
            }

            Vector3 perpendicularToDirection = new Vector3(bx, by, 0);


            var startsPointsList = BresenhamLine((int)Math.Round(startPoint.X + _divisions / 2 - bx, 0), (int)Math.Round(startPoint.Y + _divisions / 2 - by, 0), (int)Math.Round(startPoint.X + _divisions / 2 + bx, 0), (int)Math.Round(startPoint.Y + _divisions / 2 + by, 0), false);

            foreach (var item in startsPointsList)
            {
                BresenhamLine(item.Item1, item.Item2, (int)(item.Item1 + direction.X), (int)(item.Item2 + direction.Y), true);
            }

        }
        private void Circle(int x1, int y1, float z1, int radius, bool isSpherical)
        {//radius in cubes (pixels)
            List<Tuple<int, int, float>> Temp = new List<Tuple<int, int, float>>();

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {


                    if (i * i + j * j < radius * radius)
                    {


                        if (isSpherical)
                        {
                            float height = (float)-Math.Sqrt(radius * radius - i * i - j * j);
                            Temp.Add(new Tuple<int, int, float>(i + x1, j + y1, height + z1));
                        }
                        else
                        {
                            Temp.Add(new Tuple<int, int, float>(i + x1, j + y1, z1));
                        }
                    }
                }
            }


            //foreach (var item in Temp)
            //{
            //    Line.Add(new Tuple<int, int>(x1+item.Item1, y1+item.Item2));
            //}




            foreach (var point in Temp)
            {
                //Line.Add(new Tuple<int, int>(x1 + point.Item1, y1 + point.Item2));
                if (Shape0.heightArray.GetLength(0) > point.Item1 && Shape0.heightArray.GetLength(1) > point.Item2 && point.Item1 >= 0 && point.Item2 >= 0) Shape0.ModifyHeightArray(point.Item1, point.Item2, point.Item3);
            }

        }

        List<Tuple<int, int>> BresenhamLine(int x1, int y1, int x2, int y2, bool modifiedBresenhamAlghorithm = true)
        {//modifiedBresenhamAlghorithm flag cut adjcent cube inslow direction, cut heigt hanget from startpoint heigth to end point height.
            //int p;
            List<Tuple<int, int>> Line = new List<Tuple<int, int>>();
            int dx, dy, kx, ky, e;

            kx = (x1 <= x2) ? 1 : -1;
            ky = (y1 <= y2) ? 1 : -1;

            dx = x2 - x1; if (dx < 0) dx = -dx;
            dy = y2 - y1; if (dy < 0) dy = -dy;

            //p = (Uint8*)screen->pixels + y1 * screen->pitch + (x1 << 2);
            //*(Uint32*)p = color;
            Line.Add(new Tuple<int, int>(x1, y1));

            if (dx >= dy)
            {
                e = dx / 2;
                for (int i = 0; i < dx; i++)
                {
                    x1 += kx;
                    e -= dy;
                    if (e < 0)
                    {
                        if (modifiedBresenhamAlghorithm) Line.Add(new Tuple<int, int>(x1, y1));
                        y1 += ky;
                        e += dx;
                    }
                    Line.Add(new Tuple<int, int>(x1, y1));

                }
            }
            else
            {
                e = dy / 2;
                for (int i = 0; i < dy; i++)
                {
                    y1 += ky;
                    e -= dx;

                    if (e < 0)
                    {
                        if (modifiedBresenhamAlghorithm) Line.Add(new Tuple<int, int>(x1, y1));
                        x1 += kx;
                        e += dy;
                    }
                    Line.Add(new Tuple<int, int>(x1, y1));

                }
            }


            if (modifiedBresenhamAlghorithm)
            {
                float tempheight= Shape0.heightArray[Line[0].Item1, Line[0].Item2];

                float delta = (Shape0.heightArray[Line[Line.Count-1].Item1, Line[Line.Count-1].Item2] - Shape0.heightArray[Line[0].Item1, Line[0].Item2])  / (Line.Count - 2);


                for (int i = 0; i < Line.Count; i++)
                {


                    if (Shape0.heightArray.GetLength(0) > Line[i].Item1 &&
                        Shape0.heightArray.GetLength(1) > Line[i].Item2 && Line[i].Item1 >= 0 && Line[i].Item2 >= 0)

                    {
                        Shape0.ModifyHeightArray(Line[i].Item1, Line[i].Item2, tempheight+i*delta);
                    }
                }
            }

            return Line;
        }
    }
}
