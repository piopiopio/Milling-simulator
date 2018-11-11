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


    Vector3 ToolCenterPositionCoordinates = new Vector3(0, 0, 80);
    Cutter _cutter; // = new Cutter(ToolCenterPositionCoordinates);
    private int _animationSpeed = 20;

    public int AnimationSpeed
    {
        get { return _animationSpeed; }
        set { _animationSpeed = value; }
    }

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

    private Vector3? frozenCutterCeneterPointPosition;

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
        if (frozenCutterCeneterPointPosition.HasValue && _showPath) {
            GL.LineWidth(3);
            GL.Begin(BeginMode.LineStrip);
            GL.Color3(0.0, 1.0, 0.0);
        
            GL.Vertex3(frozenCutterCeneterPointPosition.Value);
            foreach (var item in _movesList)
            {


                GL.Vertex3(VBOHelpers.ConvertToOpenGLSpace(item._moveToPoint));
            }

            GL.End();
        }
    }

    private FileReader _fileReader = new FileReader();

    private ObservableCollection<LinearMillingMove> _movesList = new ObservableCollection<LinearMillingMove>();

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

        CutterIsSpherical = _fileReader.CutterIsSpherical;
        CutterDiameter = _fileReader.CutterDiameter;
        frozenCutterCeneterPointPosition = Cutter1._centerPoint;

      
    }

    private DispatcherTimer timer;
    int _stepNumber = 0;

    public int StepNumber
    {
        get
        {
            return _stepNumber;
        }
        set
        {
            _stepNumber = value;
            OnPropertyChanged(nameof(StepNumber));
        }
    }
    private List<LinearMillingMove> SimulationTemporaryList = new List<LinearMillingMove>();
    private bool _simulationResultButtonIsEnabled=true;

    public bool SimulationResultButtonIsEnabled
    {
        get { return _simulationResultButtonIsEnabled; }
        set
        {
            _simulationResultButtonIsEnabled = value;
            OnPropertyChanged(nameof(SimulationResultButtonIsEnabled));
        }
    }

    private bool _simulationStartButtonIsEnabled = true;

    public bool SimulationStartButtonIsEnabled
    {
        get { return _simulationStartButtonIsEnabled; }
        set
        {
            _simulationStartButtonIsEnabled = value;
            OnPropertyChanged(nameof(SimulationStartButtonIsEnabled));
        }
    }

    private bool _loadPathButtonIsEnabled = true;

    public bool LoadPathButtonIsEnabled
    {
        get { return _loadPathButtonIsEnabled; }
        set
        {
            _loadPathButtonIsEnabled = value;
            OnPropertyChanged(nameof(LoadPathButtonIsEnabled));
        }
    }
    public void StartSimulation()
    {
        if (_movesList.Any())
        {
            SimulationResultButtonIsEnabled = false;
            SimulationStartButtonIsEnabled = false;
            LoadPathButtonIsEnabled = false;
            StepNumber = 0;
            SimulationTemporaryList.Clear();


            SimulationTemporaryList = _movesList.ToList();
            _progress = 0;
            timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Interval = TimeSpan.FromMilliseconds(100/(double) _animationSpeed);
            timer.Tick += TimerOnTick;
            timer.Start();
        }
    }


    private double _progress = 0;

    public double Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }

    private void TimerOnTick(object sender, EventArgs e)
    {
        timer.Interval = TimeSpan.FromMilliseconds(100 / (double)_animationSpeed);
        // stopWatch.Stop();
        //MessageBox.Show(stopWatch.ElapsedMilliseconds.ToString());
        // ToolCenterPositionCoordinates.Y += 1f;

        //  stopWatch=new Stopwatch();
        //  stopWatch.Start();
        //Cutter1.CenterPoint = ToolCenterPositionCoordinates;
        //Cutter1.Draw();
        Progress = (double)StepNumber / (double)SimulationTemporaryList.Count;

        while ((SimulationTemporaryList[StepNumber]._moveToPoint - ToolCenterPositionCoordinates).Length > AnimationSpeed)
        {
            var tempVector = 0.5f * (SimulationTemporaryList[StepNumber]._moveToPoint + ToolCenterPositionCoordinates);
            var tempLinearMillingMove = new LinearMillingMove(tempVector, SimulationTemporaryList[StepNumber].LineNumber, SimulationTemporaryList[StepNumber].LinearMoveCommandNumber);

            SimulationTemporaryList.Insert(StepNumber, tempLinearMillingMove);

        }

       bool error= Material1.Cut(ToolCenterPositionCoordinates, SimulationTemporaryList[StepNumber], diameter: CutterDiameter, isSpherical: _cutterIsSphercal);
        ToolCenterPositionCoordinates = SimulationTemporaryList[StepNumber]._moveToPoint;


        Cutter1.CenterPoint = ToolCenterPositionCoordinates;
        Material1.ApplyChanges();

        if (StepNumber < SimulationTemporaryList.Count - 1)
        {
            StepNumber++;

        }
        else
        {
            timer.Stop();
            Material1.ClearErrorLastLineNumber();
            SimulationResultButtonIsEnabled = true;
            SimulationStartButtonIsEnabled = true;
            LoadPathButtonIsEnabled = true;

        }


        if (stopSimulationFlag || error)
        {
            stopSimulationFlag = false;
            ToolCenterPositionCoordinates = SimulationTemporaryList.Last()._moveToPoint;
            Cutter1.CenterPoint = ToolCenterPositionCoordinates;
            Progress = 1;
            
            timer.Stop();
            Material1.ClearErrorLastLineNumber();
            SimulationResultButtonIsEnabled = true;
            SimulationStartButtonIsEnabled = true;
            LoadPathButtonIsEnabled = true;
        }
    }

    private float _cutterDiameter = 20;
    public float CutterDiameter
    {
        get { return _cutterDiameter; }
        set
        {
            _cutterDiameter = value;
            Cutter1.CutterDiameter = value;
            OnPropertyChanged(nameof(CutterDiameter));
        }
    }

    public void SimulationResult()
    {
        //Progress = 0;
        //double moveListCount = _movesList.Count;
        //double progressIterator = 0;
        foreach (var item in _movesList)
        {
            if (Material1.Cut(ToolCenterPositionCoordinates, item, diameter: CutterDiameter,
                isSpherical: _cutterIsSphercal))
            {
                Material1.ApplyChanges();
                Refresh();
                return;
            }
            ToolCenterPositionCoordinates = item._moveToPoint;
            //progressIterator += 1;
            //Progress = progressIterator / moveListCount;
        }
        Cutter1.CenterPoint = ToolCenterPositionCoordinates;
        Material1.ApplyChanges();
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

    public void ResetMaterial()
    {
        Material1.Reset();
    }

    private bool _showPath=false;
    public bool ShowPath
    {
        get { return _showPath; }
        set
        {
            _showPath = value;
            OnPropertyChanged(nameof(ShowPath));
            Refresh();
        }
    }

    private bool stopSimulationFlag = false;
    public void StopSimulation()
    {
        stopSimulationFlag = true;



    }
}