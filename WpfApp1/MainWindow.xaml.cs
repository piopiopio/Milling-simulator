using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Threading;
using ModelowanieGeometryczne.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        #region Constructors and Destructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel1;



            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += TimerOnTick;
            timer.Start();
            MainViewModel1.MillingSimulator1.OnLoad();
            var Width = 1500;
            var Height = 900;

            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            var p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1.0f, 64.0f);
            GL.LoadMatrix(ref p);

            GL.MatrixMode(MatrixMode.Modelview);
            var mv = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref mv);



            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            float[] light_position = { 0, 80, 0 };
            float[] light_diffuse = { 0.2f, 0.1f, 0.1f };
            float[] light_ambient = { 0.2f, 0.1f, 0.1f };
            float[] light_specular = { 0.2f, 0.1f, 0.1f };
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Specular, light_specular);


            GL.Enable(EnableCap.Light0);


            MainViewModel1.MillingSimulator1.Cutter1.RefreshScene += Scene_RefreshScene;
        }

        #endregion

        #region Fields

        private MainViewModel MainViewModel1 = new MainViewModel();

        //public MainViewModel MainViewModel1
        //{
        //    get { return _mainViewModel; }
        //    set
        //    {
        //        _mainViewModel = value;

        //    }
        //}


        private int frames;

        private GLControl glControl;

        private DateTime lastMeasureTime;

        #endregion

        #region Methods

        private void GlControlOnPaint(object sender, PaintEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            ////  glControl.MakeCurrent();
            // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //  GL.LoadIdentity();
            //  GL.MatrixMode(MatrixMode.Projection);
            //    GL.LoadIdentity();
            // var halfWidth = glControl.Width / 2;
            //var halfHeight = (float)(glControl.Height / 2);
            //  GL.Ortho(-10, 10, 10, -10, 10, -10);
            GL.Viewport(0, 0, glControl.Size.Width, glControl.Size.Height);
            //double Scale = 5;
            //GL.Scale(Scale,Scale,Scale);
            //renderer.Render();



            //  GL.Viewport(0, 0, Width, Height);
            MainViewModel1.MillingSimulator1.OnUpdateFrame();
            MainViewModel1.MillingSimulator1.OnRenderFrame(_alphaX, _alphaY, _alphaZ);


            glControl.SwapBuffers();

            frames++;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(lastMeasureTime) > TimeSpan.FromSeconds(1))
            {
                Title = "PUSN: " + frames + "fps";
                frames = 0;
                lastMeasureTime = DateTime.Now;
            }

            // glControl.Invalidate();
        }

        void Scene_RefreshScene(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            glControl.Invalidate();
        }

        private void _glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            MainViewModel1.MillingSimulator1.Scale += e.Delta;
            Refresh();
        }

        private double eX;
        private double eY;

        private void _glControl_MouseDown(object sender, MouseEventArgs e)
        {
            eX = e.X;
            eY = e.Y;
        }

        private void _glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MouseMoveRotate(e.X, e.Y);
                Refresh();
            }
        }

        private double _fi;
        private double _teta;
        private double _alphaX, _alphaY, _alphaZ;




        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var temp = e.GetPosition(this);
            eX = temp.X;
            eY = temp.Y;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var temp = e.GetPosition(this);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                MouseMoveRotate((int)temp.X, (int)temp.Y);
                Refresh();
            }
        }

        private void MouseMoveRotate(int fi, int teta)
        {
            _fi = fi - eX;
            _teta = teta - eY;
            eX = fi;
            eY = teta;

            _alphaX += 16 * 4 * _teta / 750;
            _alphaY += 16 * 4 * _fi / 1440;
            _alphaZ += 0;
        }


        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainViewModel1.MillingSimulator1.Scale += e.Delta / 100000.0;
            Refresh();
        }

        private void LoadPath_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.MillingSimulator1.LoadPath();
        }

        private void OpentkWindow_OnInitialized(object sender, EventArgs e)
        {
            glControl = new GLControl();
            //glControl.MakeCurrent();
            glControl.TopLevel = false;
            glControl.Paint += GlControlOnPaint;
            // glControl.MouseWheel += _glControl_MouseWheel;
            glControl.MouseDown += _glControl_MouseDown;
            glControl.MouseMove += _glControl_MouseMove;
            glControl.MouseWheel += GlControl_MouseWheel;
            (sender as WindowsFormsHost).Child = glControl;
        }
        void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            return;
        }

        private void StartSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.MillingSimulator1.StartSimulation();
        }

        private void DefaultView_OnClick(object sender, RoutedEventArgs e)
        {
            _alphaX = 0;
            _alphaY = 0;
            _alphaZ = 0;

            glControl.Invalidate();
        }

        private void SimulationResult_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.MillingSimulator1.SimulationResult();
        }
    }

    #endregion
}