using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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
    public MillingSimulator()
    {
        _cutter = new Cutter(ToolCenterPositionCoordinates);
    }
    Vector3 ToolCenterPositionCoordinates=new Vector3(0,100,0);
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
        Material1.OnUpdateFrame();
        Cutter1.OnUpdateFrame();
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

    private ObservableCollection<LinearMillingMove> _movesList;

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
            MovesList=_fileReader.ParseGCode(op.FileName);
        }

        Cutter1.CutterIsSpherical = _fileReader.CutterIsSpherical;
        Cutter1.CutterDiameter = _fileReader.CutterDiameter;

    }

    public void StartSimulation()
    {

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(30);
        timer.Tick += TimerOnTick;
        timer.Start();
    }

    private void TimerOnTick(object sender, EventArgs e)
    {
        ToolCenterPositionCoordinates.Y += 1f;
        Cutter1.CenterPoint = ToolCenterPositionCoordinates;
        //Cutter1.Draw();
    }


}