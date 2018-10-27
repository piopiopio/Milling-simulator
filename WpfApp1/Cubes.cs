
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTK;
using System.Runtime.InteropServices;
using OpenTK.Graphics;

namespace Examples.Shapes
{


    public class Cubes : Shape
    {


        private double _materialWidth;
        private double _materialHeight;
        private double _materialDepth;
        private int _divisions;


        private float a;
        private float b;
        private float width_of_single_cube;
        private float depth_of_single_cube;
        public float[,] heightArray;

        //public float[,] HeightArray
        //{
        //    get { return heightArray; }
        //    set { }
        //}


        public void RefreshVertices()
        {
            var c = width_of_single_cube / 2;
            var d = depth_of_single_cube / 2;

            a = (float)-_materialWidth / 2 + c;
            b = (float)-_materialDepth / 2 + d;

            for (int i = 0; i < _divisions; i++)
            {
                for (int j = 0; j < _divisions; j++)
                {

                    Vertices[i * 24 + j * 24 * _divisions + 0] = new Vector3(-c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 1] = new Vector3(c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 2] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 3] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 4] = new Vector3(-c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 5] = new Vector3(c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 6] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 7] = new Vector3(-c + a, 0, -d + b);

                    Vertices[i * 24 + j * 24 * _divisions + 0 + 8] = new Vector3(-c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 1 + 8] = new Vector3(c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 2 + 8] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 3 + 8] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 4 + 8] = new Vector3(-c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 5 + 8] = new Vector3(c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 6 + 8] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 7 + 8] = new Vector3(-c + a, 0, -d + b);

                    Vertices[i * 24 + j * 24 * _divisions + 0 + 16] = new Vector3(-c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 1 + 16] = new Vector3(c + a, heightArray[i, j], d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 2 + 16] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 3 + 16] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 4 + 16] = new Vector3(-c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 5 + 16] = new Vector3(c + a, heightArray[i, j], -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 6 + 16] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisions + 7 + 16] = new Vector3(-c + a, 0, -d + b);
                    a += width_of_single_cube;
                }


                a = (float)-_materialWidth / 2 + c;
                b += depth_of_single_cube;
            }
        }

        public Cubes(double materialWidth, double materialHeight, double materialDepth, int divisions)
        {

          _materialWidth=materialWidth;
            _materialHeight=materialHeight;
            _materialDepth = materialDepth;
            _divisions = divisions;

            heightArray = new float[_divisions, _divisions];
            width_of_single_cube = (float)_materialWidth / _divisions;
            depth_of_single_cube = (float)_materialDepth / _divisions;

            Vertices = new Vector3[_divisions * _divisions * 8 * 3];
            InitializeHeightArray((float)materialHeight);
            RefreshVertices();

            //RandomHeigts();

            int[] IndicesTemp = new int[]
            {
                0+16, 1+16, 2+16, 3+16, //z
                3, 2, 6, 7, // y
                7+16, 6+16, 5+16, 4+16, //zz
                4, 5, 1, 0, //-y
                5+8, 6+8, 2+8, 1+8, //x
                7+8, 4+8, 0+8, 3+8  //-x

                //0, 1, 2, 3, //z
                //3, 2, 6, 7, // y
                //7, 6, 5, 4, //zz
                //4, 5, 1, 0, //-y
                //5, 6, 2, 1, //x
                //7, 4, 0, 3  //-x
            };


            Indices = new int[IndicesTemp.Length * _divisions * _divisions];

            for (int i = 0; i < _divisions * _divisions; i++)
            {
                for (int j = 0; j < IndicesTemp.Length; j++)
                {
                    Indices[i * IndicesTemp.Length + j] = IndicesTemp[j] + 24 * i;
                }
            }

            // Normals = new Vector3[Pieces * Pieces * 8];
            Normals = new Vector3[_divisions * _divisions * 8 * 3];
            for (int i = 0; i < _divisions * _divisions; i++)
            //for (int i = 0; i < Pieces * Pieces; i++)
            {
                Normals[i * 24 + 0] = new Vector3(0.0f, 1.0f, 0.0f);
                Normals[i * 24 + 1] = new Vector3(0.0f, 1.0f, 0.0f);
                Normals[i * 24 + 2] = new Vector3(0.0f, -1.0f, 0.0f);
                Normals[i * 24 + 3] = new Vector3(0.0f, -1.0f, 0.0f);
                Normals[i * 24 + 4] = new Vector3(0.0f, 1.0f, 0.0f);
                Normals[i * 24 + 5] = new Vector3(0.0f, 1.0f, 0.0f);
                Normals[i * 24 + 6] = new Vector3(0.0f, -1.0f, 0.0f);
                Normals[i * 24 + 7] = new Vector3(0.0f, -1.0f, 0.0f);


                Normals[i * 24 + 0 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 1 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 2 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 3 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 4 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 5 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 6 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
                Normals[i * 24 + 7 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);


                Normals[i * 24 + 0 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
                Normals[i * 24 + 1 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
                Normals[i * 24 + 2 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
                Normals[i * 24 + 3 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
                Normals[i * 24 + 4 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
                Normals[i * 24 + 5 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
                Normals[i * 24 + 6 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
                Normals[i * 24 + 7 + 16] = new Vector3(0.0f, 0.0f, -1.0f);


            };

            Colors = new int[_divisions * _divisions * 8 * 3];
            for (int i = 0; i < _divisions * _divisions; i++)
            {
                Colors[i * 24 + 0] = Utilities.ColorToRgba32(Color.Coral);
                Colors[i * 24 + 1] = Utilities.ColorToRgba32(Color.Blue);
                Colors[i * 24 + 2] = Utilities.ColorToRgba32(Color.Chartreuse);
                Colors[i * 24 + 3] = Utilities.ColorToRgba32(Color.Green);
                Colors[i * 24 + 4] = Utilities.ColorToRgba32(Color.Fuchsia);
                Colors[i * 24 + 5] = Utilities.ColorToRgba32(Color.Gray);
                Colors[i * 24 + 6] = Utilities.ColorToRgba32(Color.Indigo);
                Colors[i * 24 + 7] = Utilities.ColorToRgba32(Color.LightBlue);

                Colors[i * 24 + 0 + 8] = Utilities.ColorToRgba32(Color.Coral);
                Colors[i * 24 + 1 + 8] = Utilities.ColorToRgba32(Color.Blue);
                Colors[i * 24 + 2 + 8] = Utilities.ColorToRgba32(Color.Chartreuse);
                Colors[i * 24 + 3 + 8] = Utilities.ColorToRgba32(Color.Green);
                Colors[i * 24 + 4 + 8] = Utilities.ColorToRgba32(Color.Fuchsia);
                Colors[i * 24 + 5 + 8] = Utilities.ColorToRgba32(Color.Gray);
                Colors[i * 24 + 6 + 8] = Utilities.ColorToRgba32(Color.Indigo);
                Colors[i * 24 + 7 + 8] = Utilities.ColorToRgba32(Color.LightBlue);

                Colors[i * 24 + 0 + 16] = Utilities.ColorToRgba32(Color.Coral);
                Colors[i * 24 + 1 + 16] = Utilities.ColorToRgba32(Color.Blue);
                Colors[i * 24 + 2 + 16] = Utilities.ColorToRgba32(Color.Chartreuse);
                Colors[i * 24 + 3 + 16] = Utilities.ColorToRgba32(Color.Green);
                Colors[i * 24 + 4 + 16] = Utilities.ColorToRgba32(Color.Fuchsia);
                Colors[i * 24 + 5 + 16] = Utilities.ColorToRgba32(Color.Gray);
                Colors[i * 24 + 6 + 16] = Utilities.ColorToRgba32(Color.Indigo);
                Colors[i * 24 + 7 + 16] = Utilities.ColorToRgba32(Color.LightBlue);
            };

            Texcoords = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };
        }

        private void InitializeHeightArray(float h)
        {

            for (int i = 0; i < _divisions; i++)
            {
                for (int j = 0; j < _divisions; j++)
                {
                    heightArray[i, j] = h;
                }
            }
            RefreshVertices();

        }

        public void ModifyHeightArray(int item1, int item2, float f)
        {
            if (heightArray[item1, item2] > f) heightArray[item1, item2] = f;
        }
    }
}
