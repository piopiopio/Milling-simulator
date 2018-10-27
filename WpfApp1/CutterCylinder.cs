
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTK;
using System.Runtime.InteropServices;

namespace Examples.Shapes2
{
    public class CutterCylinder : Shape
    {
        Vector3 _centerPoint= new Vector3(0, 0, 0);
        List<int> indices;
        List<Vector3>vertices;
        public Vector3 CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                _centerPoint = value;
                RefreshVertices();
            }
        }

        float _radius;

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                CreateCylinder(out indices);
                RefreshVertices();
            }
        }
        private int CylinderDivisions = 20;

        private double Height;


        private void RefreshVertices()
        {









            //for (int i = 0; i < Vertices.Length; i++)
            //{
            //    Vertices[i] += CenterPoint;
            //}


            for (int i = 0; i < Vertices.Length; i++)
            {
                
                Vertices[i] = vertices[i] + CenterPoint;
            }


        }

        private List<Vector3> CreateCylinder(out List<int> indices)
        {
            vertices = new List<Vector3>();
            for (double y = 0; y < 2; y++)
                for (double x = 0; x < CylinderDivisions; x++)
                {
                    double theta = (x / (CylinderDivisions - 1)) * 2 * Math.PI;

                    vertices.Add(new Vector3
                    {
                        X = (float)(Radius * Math.Cos(theta)),
                        Y = (float)(Height * y),
                        Z = (float)(Radius * Math.Sin(theta)),
                    });
                }

            indices = new List<int>();
            for (int x = 0; x < CylinderDivisions - 1; x++)
            {
                indices.Add(x);
                indices.Add(x + CylinderDivisions);
                indices.Add(x + CylinderDivisions + 1);

                indices.Add(x + CylinderDivisions + 1);
                indices.Add(x + 1);
                indices.Add(x);
            }

            Vertices = vertices.ToArray();
            Indices = indices.ToArray();

            return vertices;
        }
        public CutterCylinder(Vector3 centerPosition, float radius)
        {
            _centerPoint = centerPosition;
            _radius = radius;
            Height = 2 * Radius;
            //width_of_single_cube = MaterialWidth / Pieces;
            //depth_of_single_cube = MaterialDepth / Pieces;

            //Vertices = new Vector3[Pieces * Pieces * 8 * 3];
            CreateCylinder(out indices);

            RefreshVertices();

            //RandomHeigts();
            //int[] IndicesTemp = new int[]
            //{
            //    0+16, 1+16, 2+16, 3+16, //z
            //    3, 2, 6, 7, // y
            //    7+16, 6+16, 5+16, 4+16, //zz
            //    4, 5, 1, 0, //-y
            //    5+8, 6+8, 2+8, 1+8, //x
            //    7+8, 4+8, 0+8, 3+8  //-x

            //    //0, 1, 2, 3, //z
            //    //3, 2, 6, 7, // y
            //    //7, 6, 5, 4, //zz
            //    //4, 5, 1, 0, //-y
            //    //5, 6, 2, 1, //x
            //    //7, 4, 0, 3  //-x
            //};


            //Indices = new int[IndicesTemp.Length * Pieces * Pieces];

            //for (int i = 0; i < Pieces * Pieces; i++)
            //{
            //    for (int j = 0; j < IndicesTemp.Length; j++)
            //    {
            //        Indices[i * IndicesTemp.Length + j] = IndicesTemp[j] + 24 * i;
            //    }
            //}

            // Normals = new Vector3[Pieces * Pieces * 8];

            //for (int i = 0; i < Pieces * Pieces; i++)
            ////for (int i = 0; i < Pieces * Pieces; i++)
            //{
            //    Normals[i * 24 + 0] = new Vector3(0.0f, 1.0f, 0.0f);
            //    Normals[i * 24 + 1] = new Vector3(0.0f, 1.0f, 0.0f);
            //    Normals[i * 24 + 2] = new Vector3(0.0f, -1.0f, 0.0f);
            //    Normals[i * 24 + 3] = new Vector3(0.0f, -1.0f, 0.0f);
            //    Normals[i * 24 + 4] = new Vector3(0.0f, 1.0f, 0.0f);
            //    Normals[i * 24 + 5] = new Vector3(0.0f, 1.0f, 0.0f);
            //    Normals[i * 24 + 6] = new Vector3(0.0f, -1.0f, 0.0f);
            //    Normals[i * 24 + 7] = new Vector3(0.0f, -1.0f, 0.0f);


            //    Normals[i * 24 + 0 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 1 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 2 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 3 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 4 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 5 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 6 + 8] = new Vector3(1.0f, 0.0f, 0.0f);
            //    Normals[i * 24 + 7 + 8] = new Vector3(-1.0f, 0.0f, 0.0f);


            //    Normals[i * 24 + 0 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
            //    Normals[i * 24 + 1 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
            //    Normals[i * 24 + 2 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
            //    Normals[i * 24 + 3 + 16] = new Vector3(0.0f, 0.0f, 1.0f);
            //    Normals[i * 24 + 4 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
            //    Normals[i * 24 + 5 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
            //    Normals[i * 24 + 6 + 16] = new Vector3(0.0f, 0.0f, -1.0f);
            //    Normals[i * 24 + 7 + 16] = new Vector3(0.0f, 0.0f, -1.0f);


            //};

            //Colors = new int[Pieces * Pieces * 8 * 3];
            //for (int i = 0; i < Pieces * Pieces; i++)
            //{
            //    Colors[i * 24 + 0] = Utilities.ColorToRgba32(Color.Coral);
            //    Colors[i * 24 + 1] = Utilities.ColorToRgba32(Color.Blue);
            //    Colors[i * 24 + 2] = Utilities.ColorToRgba32(Color.Chartreuse);
            //    Colors[i * 24 + 3] = Utilities.ColorToRgba32(Color.Green);
            //    Colors[i * 24 + 4] = Utilities.ColorToRgba32(Color.Fuchsia);
            //    Colors[i * 24 + 5] = Utilities.ColorToRgba32(Color.Gray);
            //    Colors[i * 24 + 6] = Utilities.ColorToRgba32(Color.Indigo);
            //    Colors[i * 24 + 7] = Utilities.ColorToRgba32(Color.LightBlue);

            //    Colors[i * 24 + 0 + 8] = Utilities.ColorToRgba32(Color.Coral);
            //    Colors[i * 24 + 1 + 8] = Utilities.ColorToRgba32(Color.Blue);
            //    Colors[i * 24 + 2 + 8] = Utilities.ColorToRgba32(Color.Chartreuse);
            //    Colors[i * 24 + 3 + 8] = Utilities.ColorToRgba32(Color.Green);
            //    Colors[i * 24 + 4 + 8] = Utilities.ColorToRgba32(Color.Fuchsia);
            //    Colors[i * 24 + 5 + 8] = Utilities.ColorToRgba32(Color.Gray);
            //    Colors[i * 24 + 6 + 8] = Utilities.ColorToRgba32(Color.Indigo);
            //    Colors[i * 24 + 7 + 8] = Utilities.ColorToRgba32(Color.LightBlue);

            //    Colors[i * 24 + 0 + 16] = Utilities.ColorToRgba32(Color.Coral);
            //    Colors[i * 24 + 1 + 16] = Utilities.ColorToRgba32(Color.Blue);
            //    Colors[i * 24 + 2 + 16] = Utilities.ColorToRgba32(Color.Chartreuse);
            //    Colors[i * 24 + 3 + 16] = Utilities.ColorToRgba32(Color.Green);
            //    Colors[i * 24 + 4 + 16] = Utilities.ColorToRgba32(Color.Fuchsia);
            //    Colors[i * 24 + 5 + 16] = Utilities.ColorToRgba32(Color.Gray);
            //    Colors[i * 24 + 6 + 16] = Utilities.ColorToRgba32(Color.Indigo);
            //    Colors[i * 24 + 7 + 16] = Utilities.ColorToRgba32(Color.LightBlue);
            //};

            //Texcoords = new Vector2[]
            //{
            //    new Vector2(0, 1),
            //    new Vector2(1, 1),
            //    new Vector2(1, 0),
            //    new Vector2(0, 0),
            //    new Vector2(0, 1),
            //    new Vector2(1, 1),
            //    new Vector2(1, 0),
            //    new Vector2(0, 0),
            //};
        }


    }
}


//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;

//using OpenTK;
//using System.Runtime.InteropServices;

//namespace Examples.Shapes
//{
//    public class Cubes : Shape
//    {
//        public Cubes()
//        {
//            Vertices = new Vector3[]
//            {
//                new Vector3(-1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f,  1.0f, -1.0f),
//                new Vector3(-1.0f,  1.0f, -1.0f),

//                new Vector3(-1.0f+2.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f+2.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f+2.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f+2.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f+2.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f+2.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f+2.0f,  1.0f, -1.0f),
//                new Vector3(-1.0f+2.0f,  1.0f, -1.0f)
//            };

//            Indices = new int[]
//            {
//                // front face
//                0, 1, 2, 2, 3, 0,
//                // top face
//                3, 2, 6, 6, 7, 3,
//                // back face
//                7, 6, 5, 5, 4, 7,
//                // left face
//                4, 0, 3, 3, 7, 4,
//                // bottom face
//                0, 1, 5, 5, 4, 0,
//                // right face
//                1, 5, 6, 6, 2, 1,

//                // front face
//                8, 9, 10, 10, 11, 8,
//                // top face
//                11, 10, 14, 14, 15, 11,
//                // back face
//                15, 14, 13, 13, 12, 15,
//                // left face
//                12, 8, 11, 11, 15, 12,
//                // bottom face
//                8, 9, 13, 13, 12, 8,
//                // right face
//                9,13, 14, 14, 10, 9,
//            };

//            Normals = new Vector3[]
//            {
//                new Vector3(-1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f,  1.0f, -1.0f),
//                new Vector3(-1.0f,  1.0f, -1.0f),

//                new Vector3(-1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f, -1.0f,  1.0f),
//                new Vector3( 1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f,  1.0f,  1.0f),
//                new Vector3(-1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f, -1.0f, -1.0f),
//                new Vector3( 1.0f,  1.0f, -1.0f),
//                new Vector3(-1.0f,  1.0f, -1.0f),
//            };

//            Colors = new int[]
//            {
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.DarkRed),
//                Utilities.ColorToRgba32(Color.Gold),
//                Utilities.ColorToRgba32(Color.Gold),
//            };

//            Texcoords = new Vector2[]
//            {
//                new Vector2(0, 1),
//                new Vector2(1, 1),
//                new Vector2(1, 0),
//                new Vector2(0, 0),
//                new Vector2(0, 1),
//                new Vector2(1, 1),
//                new Vector2(1, 0),
//                new Vector2(0, 0),
//                new Vector2(0, 1),
//                new Vector2(1, 1),
//                new Vector2(1, 0),
//                new Vector2(0, 0),
//                new Vector2(0, 1),
//                new Vector2(1, 1),
//                new Vector2(1, 0),
//                new Vector2(0, 0),
//            };
//        }


//    }
//}