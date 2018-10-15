
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTK;
using System.Runtime.InteropServices;

namespace Examples.Shapes1
{
    public class CutterSphere : Shape
    {
        Vector3 _centerPoint = new Vector3(0, 0, 0);
        public Vector3 CenterPoint
        {

            get { return _centerPoint; }
            set
            {
                _centerPoint = value;
                RefreshVertices();
            }
        }

        float _radius = 80;

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                RefreshVertices();
            }
        }

        private void RefreshVertices()
        {

            int SphereDivisions = 20;


            var vertices = new List<Vector3>();
            for (double y = 0; y < SphereDivisions; y++)
            {
                double phi = (y / (SphereDivisions - 1)) * Math.PI;
                for (double x = 0; x < SphereDivisions; x++)
                {
                    double theta = (x / (SphereDivisions - 1)) * 2 * Math.PI;

                    vertices.Add(new Vector3(
                        Radius * (float)Math.Sin(phi) * (float)Math.Cos(theta),
                        Radius * (float)Math.Cos(phi),
                        Radius * (float)Math.Sin(phi) * (float)Math.Sin(theta)));
                }
            }

            var indices = new List<int>();
            for (int y = 0; y < SphereDivisions - 1; y++)
                for (int x = 0; x < SphereDivisions - 1; x++)
                {
                    indices.Add((y + 0) * SphereDivisions + x);
                    indices.Add((y + 1) * SphereDivisions + x);
                    indices.Add((y + 1) * SphereDivisions + x + 1);

                    indices.Add((y + 1) * SphereDivisions + x + 1);
                    indices.Add((y + 0) * SphereDivisions + x + 1);
                    indices.Add((y + 0) * SphereDivisions + x);
                }


            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
           // Normals = new Vector3[vertices.Count];


            for (int j = 0; j < vertices.Count; j++)
            {
               // var vertex = vertices[j];
              //  Normals[j] = new Vector3(-vertex.X / Radius, -vertex.Y / Radius, -vertex.Z / Radius);
                Vertices[j] += CenterPoint;               
            }

        }


        public CutterSphere(Vector3 centerPosition, float radius)
        {
            _centerPoint = centerPosition;
            _radius = radius;
            RefreshVertices();
            
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