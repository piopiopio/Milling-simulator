using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using Examples.Shapes;
using Examples.Shapes1;
using Examples.Shapes2;
using ModelowanieGeometryczne.Helpers;
using ModelowanieGeometryczne.Model;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using WpfApp1;


public class MillingSimulator : ViewModelBase
{
    //  private Stopwatch stopWatch;
    public MillingSimulator()
    {
        //  stopWatch = new Stopwatch();
        //  stopWatch.Start();
        _cutter = new Cutter(VBOHelpers.ConvertToOpenGLSpace(ToolCenterPositionCoordinates));
    }
    Vector3 ToolCenterPositionCoordinates = new Vector3(0 ,0, 100);
    Cutter _cutter;// = new Cutter(ToolCenterPositionCoordinates);


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
    }

    public void OnUpdateFrame()
    {
        //Material1.OnUpdateFrame();
        //Cutter1.OnUpdateFrame();
    }

    private double counter = 0;
    private double scale = 0.01;

    public double Scale
    {
        get => scale;
        set => scale = Math.Max(value, 0.0001);
    }

    public void OnRenderFrame(double alphaX, double alphaY, double alphaZ)
    {
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

        Material1.Draw();
        Cutter1.Draw();
    }

    private FileReader _fileReader = new FileReader();

    private ObservableCollection<LinearMillingMove> _movesList=new ObservableCollection<LinearMillingMove>();

    public ObservableCollection<LinearMillingMove> MovesList
    {
        get { return _movesList; }
        set
        {
            _movesList = value;
            OnPropertyChanged(nameof(MovesList));
        }
    }
    public void LoadPath()
    {
        OpenFileDialog op = new OpenFileDialog();
        op.Title = "Open";
        if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            MovesList = _fileReader.ParseGCode(op.FileName);
        }

        Cutter1.CutterIsSpherical = _fileReader.CutterIsSpherical;
        Cutter1.CutterDiameter = _fileReader.CutterDiameter;

    }

    private DispatcherTimer timer;
    public void StartSimulation()
    {

        ////debug
        //timer = new DispatcherTimer();
        //timer.Interval = TimeSpan.FromMilliseconds(30);
        //timer.Tick += TimerOnTick;
        //timer.Start();

        
    }


    private void TimerOnTick(object sender, EventArgs e)
    {
        // stopWatch.Stop();
        //MessageBox.Show(stopWatch.ElapsedMilliseconds.ToString());
        ToolCenterPositionCoordinates.Y += 1f;

        //  stopWatch=new Stopwatch();
        //  stopWatch.Start();
        Cutter1.CenterPoint = ToolCenterPositionCoordinates;
        //Cutter1.Draw();

    }

    public double CutterDiameter = 50;

    public void SimulationResult()
    {
        //if (MovesList.Any())
        //{
        //    MessageBox.Show("nie pusto");
        //}
        //else
        //{
        //    MessageBox.Show("pusto");

        //}
        Cutter1.CenterPoint = ToolCenterPositionCoordinates + new Vector3(0,0,100);
        //Material1.Cut(startPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(0, 0, 40f)), endPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(50, 2, 20f)), diameter:CutterDiameter, isSpherical:_cutterIsSphercal);
        Material1.Cut(new Vector3(0, 0, 40f), new Vector3(50, 50, 70f), diameter:CutterDiameter, isSpherical:_cutterIsSphercal);
       // Material1.Cut(startPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(-3, -2, 0.1f)), endPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(3, 2, 0.1f)), diameter:CutterDiameter, isSpherical:false);
        //Material1.Cut(startPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(-3, -1, 0.1f)), endPoint:VBOHelpers.ConvertToOpenGLSpace(new Vector3(3, 3, 0.1f)), diameter:CutterDiameter, isSpherical:false);

    }

    private bool _cutterIsSphercal = false;

    public bool CutterIsSpherical
    {
        get { return _cutterIsSphercal; }
        set
        {
            _cutterIsSphercal = value;
            Cutter1.CutterIsSpherical = _cutterIsSphercal;

            
            OnUpdateFrame();
            OnPropertyChanged(nameof(CutterIsSpherical));
        

        }
    }
}