using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Examples.Shapes;
using Examples.Shapes1;
using Examples.Shapes2;
using ModelowanieGeometryczne.Model;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WpfApp1;

/// <summary>
///     Generates Array Buffers Objects for Vertices, Normals, TexCoords, Colors, and primitive Indices.
///     The uses DrawElements to draw the primitives from the Array Buffers.
///     Also shows an example of dynamic updating of VBOs.
/// </summary>
public class MillingSimulator : ViewModelBase
{
    #region OnLoad override


    public MillingSimulator()
    {

    }

    Cutter _cutter = new Cutter();

    public Cutter Cutter1
    {
        get { return _cutter; }
        set
        {
            _cutter = value;
            OnPropertyChanged(nameof(Cutter1));
        }
    }


    Material _material = new Material();

    public Material Material1
    {
        get { return _material; }
        set
        {
            _material = value;
            OnPropertyChanged(nameof(Material1));
        }
    }




    public void OnLoad()
    {
        Cutter1.OnLoad();
        Material1.OnLoad();
        //base.OnLoad(e);

        //GL.ClearColor(0.1f, 0.1f, 0.5f, 0.0f);
        // GL.Enable(EnableCap.DepthTest);

        // Vertex Buffers
        //vbo = VBOHelpers.LoadVBO(shape);

        //vbo1 = LoadVBO(shape1);
        //vbo2 = LoadVBO(shape2);

        // Lighting
        //GL.Enable(EnableCap.Light0);
        //GL.Enable(EnableCap.Lighting);

        //// Texture
        //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

        //GL.GenTextures(1, out textureID);
        //GL.BindTexture(TextureTarget.Texture2D, textureID);
        //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        //Bitmap bitmap = new Bitmap("D:\\Users\\Piotr\\documents\\visual studio 2017\\Projects\\ConsoleApp1 - Copy\\Data\\logo.jpg");
        //BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //{
        //    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
        //        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        //}
        //bitmap.UnlockBits(data);

        //GL.Enable(EnableCap.Texture2D);
    }

    #endregion

    #region OnResize override

    protected void OnResize(EventArgs e)
    {
        //TODO: Width, Height;
        var Width = 500;
        var Height = 500;

        GL.Viewport(0, 0, Width, Height);

        GL.MatrixMode(MatrixMode.Projection);
        var p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 1.0f, 64.0f);
        GL.LoadMatrix(ref p);

        GL.MatrixMode(MatrixMode.Modelview);
        var mv = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
        GL.LoadMatrix(ref mv);
    }

    #endregion

    public void OnUpdateFrame()
    {

        // Method 1 (BufferData) : Modify local copy and resend data
        //Random rnd =new Random();


        Material1.OnUpdateFrame();
        Cutter1.OnUpdateFrame();




    }

    /// <summary>
    ///     Generate a VertexBuffer for each of Color, Normal, TextureCoordinate, Vertex, and Indices
    /// </summary>
   


    #region public static void Main()

    /// <summary>
    ///     Entry point of this example.
    /// </summary>
    //[STAThread]
    //public static void Main1()
    //{
    //    using (VBO_Renderer example = new VBO_Renderer())
    //    {
    //        // Get the title and category of this example using reflection.
    //        //example.Title = String.Format("OpenTK | {0} {1}: {2}", 1, 2, 3);
    //        //example.Run();
    //    }
    //}

    #endregion




    #region --- Private Fields ---




    private VBOHelpers.Vbo vbo;


    private int textureID;

    // Flag to indicate it the Array Buffer should dynamically update
    private readonly bool dynamicUpdate = true;


    //private void OnKeyDownInternal(object sender, KeyboardKeyEventArgs e) { OnKeyDown(e); }

    ///// <summary>
    ///// Occurs whenever a keybord key is pressed.
    ///// </summary>
    //protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
    //{
    //    switch (e.Key)
    //    {
    //        // Lighting
    //        case OpenTK.Input.Key.L:
    //            if (GL.IsEnabled(EnableCap.Lighting)) GL.Disable(EnableCap.Lighting);
    //            else GL.Enable(EnableCap.Lighting);
    //            break;

    //        // Texture
    //        case OpenTK.Input.Key.T:
    //            if (GL.IsEnabled(EnableCap.Texture2D)) GL.Disable(EnableCap.Texture2D);
    //            else GL.Enable(EnableCap.Texture2D);
    //            break;

    //        // Dynamic Update
    //        case OpenTK.Input.Key.D:
    //            dynamicUpdate = !dynamicUpdate;
    //            break;

    //        // Exit
    //        case OpenTK.Input.Key.Escape:
    //            //Exit();
    //            break;
    //    }
    //}

    #endregion

    #region OnRenderFrame

    private double counter = 0;
    private double scale = 0.01;

    public double Scale
    {
        get => scale;
        set => scale = Math.Max(value, 0.0001);
    }

    Vector3 _cutterPosition;

    public Vector3 CutterPosition
    {
        get { return _cutterPosition; }
        set
        {
            _cutterPosition = value;
            Cutter1.CenterPoint = _cutterPosition;

        }
    }





    public void OnRenderFrame(double alphaX, double alphaY, double alphaZ)
    {
        //base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.PushAttrib(AttribMask.LightingBit);
        GL.PushAttrib(AttribMask.LightingBit);
        GL.Disable(EnableCap.Lighting);
        GL.Color3(Color.White);

        GL.PopAttrib();

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        var modelview = Matrix4.LookAt(0.0f, 3.5f, 3.5f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
        GL.LoadMatrix(ref modelview);

        //counter += e.Time;
        // counter += 0.1;
        //  GL.Rotate(counter * 10, 0, 1, 0);
        GL.Rotate(alphaX, 1, 0, 0);
        GL.Rotate(alphaY, 0, 1, 0);
        GL.Rotate(alphaX, 0, 0, 1);

        GL.Scale(Scale, Scale, Scale);
        GL.Rotate(50, 0, 1, 0);


        //VBOHelpers.Draw(vbo, "Quad");
        //Draw(vbo1, "QuadStrip");
        //Draw(vbo2, "Triangles");

        Material1.Draw();
        Cutter1.Draw();


    }

    private FileReader _fileReader = new FileReader();
    public FileReader FileReader
    {
        get { return _fileReader; }
        set
        {
            _fileReader = value;
            OnPropertyChanged(nameof(FileReader));
        }
    }
    #endregion

    public void LoadPath()
    {
        OpenFileDialog op = new OpenFileDialog();
        op.Title = "Open";
        if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {

            _fileReader.ParseGCode(op.FileName);

        }
    }
}