
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
        private int _divisionsWidth;
        private int _divisionsDepth;
        //private int _divisions;


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

            for (int i = 0; i < _divisionsDepth; i++)
            {
                for (int j = 0; j < _divisionsWidth; j++)
                {

                    Vertices[i * 24 + j * 24 * _divisionsDepth + 0] = new Vector3(-c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 1] = new Vector3(c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 2] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 3] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 4] = new Vector3(-c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 5] = new Vector3(c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 6] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 7] = new Vector3(-c + a, 0, -d + b);
                                                         
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 0 + 8] = new Vector3(-c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 1 + 8] = new Vector3(c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 2 + 8] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 3 + 8] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 4 + 8] = new Vector3(-c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 5 + 8] = new Vector3(c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 6 + 8] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 7 + 8] = new Vector3(-c + a, 0, -d + b);
                                                         
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 0 + 16] = new Vector3(-c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 1 + 16] = new Vector3(c + a, heightArray[ j,i], d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 2 + 16] = new Vector3(c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 3 + 16] = new Vector3(-c + a, 0, d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 4 + 16] = new Vector3(-c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 5 + 16] = new Vector3(c + a, heightArray[ j,i], -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 6 + 16] = new Vector3(c + a, 0, -d + b);
                    Vertices[i * 24 + j * 24 * _divisionsDepth + 7 + 16] = new Vector3(-c + a, 0, -d + b);
                    a += width_of_single_cube;
                }


                a = (float)-_materialWidth / 2 + c;
                b += depth_of_single_cube;
            }
        }

        public Cubes(double materialWidth, double materialHeight, double materialDepth, int divisionsWidth, int divisionsDepth)
        {

          _materialWidth=materialWidth;
            _materialHeight=materialHeight;
            _materialDepth = materialDepth;
            _divisionsWidth = divisionsWidth;
            _divisionsDepth = divisionsDepth;
           // _divisions = divisions;
            heightArray = new float[_divisionsWidth, _divisionsDepth];
            width_of_single_cube = (float)_materialWidth / _divisionsWidth;
            depth_of_single_cube = (float)_materialDepth / _divisionsDepth;

            Vertices = new Vector3[_divisionsWidth * _divisionsDepth * 8 * 3];
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


            Indices = new int[IndicesTemp.Length * _divisionsWidth * _divisionsDepth];

            for (int i = 0; i < _divisionsWidth * _divisionsDepth; i++)
            {
                for (int j = 0; j < IndicesTemp.Length; j++)
                {
                    Indices[i * IndicesTemp.Length + j] = IndicesTemp[j] + 24 * i;
                }
            }

            // Normals = new Vector3[Pieces * Pieces * 8];
            Normals = new Vector3[_divisionsWidth * _divisionsDepth * 8 * 3];
            for (int i = 0; i < _divisionsWidth * _divisionsDepth; i++)
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

        }

        private void InitializeHeightArray(float h)
        {

            for (int i = 0; i < _divisionsWidth; i++)
            {
                for (int j = 0; j < _divisionsDepth; j++)
                {
                    heightArray[i, j] = h;
                }
            }
            RefreshVertices();

        }

        public bool ModifyHeightArray(int item1, int item2, float f)
        {//Return if cutter is in material.
            if (heightArray[item1, item2] > f)
            {
                heightArray[item1, item2] = f;
                return true;
            }

            return false;
        }
    }
}
