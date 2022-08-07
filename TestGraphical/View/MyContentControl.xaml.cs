using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Commands;
using TestGraphical.Controls;
using TestGraphical.ViewModel;

namespace TestGraphical.View
{
    /// <summary>
    /// Interaction logic for ContentControl.xaml
    /// </summary>
    public partial class MyContentControl : UserControl
    {
        public MyContentControl()
        {
            InitializeComponent();
            Events.AddStepToCanvasEvt.Subscribe(AddControl);
            Events.RefreshControlsEvt.Subscribe(RefreshControl);
            Events.ControlClicked.Subscribe(ChildControlClicked);
            Events.ConnectExperimental.Subscribe(ConnectExperimental);
        }




        private void ConnectExperimental()
        {
            Point GetPoint(UIElement e) => e.TransformToAncestor(MyCanvas).Transform(new Point(0, 0));
            var xs = MyCanvas.Children.OfType<StepControl>().ToList();
            var res = xs.Select(s => (s, xs.Where(s1 => s1 != s))).SelectMany(t => t.Item2.Select(a => (t.s, a)))
                .SelectMany(t => t.a.OutputsPanel.Children.OfType<StepOutput>().Select(o => (t.s, o))).Select(p => (GetPoint(p.s), GetPoint(p.o)))
                .Select(pair => new Line() { X1 = pair.Item1.X, Y1 = pair.Item1.Y, X2 = pair.Item2.X, Y2 = pair.Item2.Y }).ToList();

            BackCanvas.Children.Clear();            
            foreach (var l in res)
            {
                l.StrokeThickness = 2;
                l.Stroke = new SolidColorBrush(Colors.Black);
                BackCanvas.Children.Add(l);
            }

        }

        Point lastClickPos;
        Point mousePos;
        private bool isDragMode = true;
        private bool isControlClicked = false;
        StepControl SelectedControl = null;

        private void ChildControlClicked(StepControl obj)
        {
            SelectedControl = obj;
            UpdateZInexesByLastCtrl(obj);
            isControlClicked = true;
            lastClickPos = (Point)((Vector)obj.TransformToAncestor(MyCanvas).Transform(new Point(0, 0)) + (Vector)obj.ClickPos);
        }


        private void CanvasClicked()
        {
            isControlClicked = false;
            isDragMode = false;
        }

        void CanvasUnclicked()
        {
            isControlClicked = true;
            isDragMode = false;
            if (SelectedControl != null)
            {
                SelectedControl.IsClicked = false;
            }
        }

        private void BackCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isControlClicked = true;
            if(SelectedControl != null)
            {
                SelectedControl.IsClicked = false;
            }
            isDragMode = false;
        }
        private void BackCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.GetPosition(MyCanvas);
            if (isControlClicked && e.LeftButton == MouseButtonState.Pressed)
            {
                var a = mousePos - lastClickPos;
                if (Math.Abs(a.X) > 40 || Math.Abs(a.Y) > 40)
                {
                    isDragMode = true;
                }
                if (isDragMode)
                {
                    if (SelectedControl != null)
                    {
                        SelectedControl.SetPosition(mousePos);
                    }
                }
            }
        }


        private void UpdateZInexesByLastCtrl(StepControl obj)
        {
            List<UIElement> ctrlList = new List<UIElement>();
            foreach (UIElement ctl in MyCanvas.Children)
            {
                ctrlList.Add(ctl);
            }
            var newZIndex = ctrlList.Select(ctl => Panel.GetZIndex(ctl)).Max() + 1;

            Panel.SetZIndex(obj, newZIndex);

            List<UIElement> ctrlList2 = new List<UIElement>();
            foreach (UIElement ctl in MyCanvas.Children)
            {
                ctrlList2.Add(ctl);
            }

            var ordered = ctrlList2.OrderBy(ctl => Panel.GetZIndex(ctl));
            var counter = 0;
            foreach(var ctl in ordered)
            {
                Panel.SetZIndex(ctl, counter);
                counter++;                
            }
        }

        private int counter = 0;
        private void AddControl(VM_Step vm_step)
        {
            Controls.StepControl stepControl = new Controls.StepControl();
            vm_step.XOffset = counter;
            vm_step.YOffset = counter;
            vm_step.Name = $"name {counter}";
            counter += 10;
            stepControl.DataContext = vm_step;
            stepControl.VisualGuid = vm_step.VisualGuid;
            MyCanvas.Children.Add(stepControl);
            UpdateZInexesByLastCtrl(stepControl);
        }

        private DelegateCommand canvasClickedCmd;
        public DelegateCommand CanvasClickedCmd =>
            canvasClickedCmd ?? (canvasClickedCmd = new DelegateCommand(CanvasClicked));


        private DelegateCommand canvasUnclickedCmd;
        public DelegateCommand CanvasUnclickedCmd =>
            canvasUnclickedCmd ?? (canvasUnclickedCmd = new DelegateCommand(CanvasUnclicked));




        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(MyContentControl), new PropertyMetadata(default));



        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseXProperty =
            DependencyProperty.Register("MouseX", typeof(double), typeof(MyContentControl), new PropertyMetadata(default));



        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseYProperty =
            DependencyProperty.Register("MouseY", typeof(double), typeof(MyContentControl), new PropertyMetadata(default));



        private void RefreshControl()
        {

        }



        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(MyContentControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)) { BindsTwoWayByDefault = true });

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mousePos = e.GetPosition(MyCanvas as IInputElement);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {

        }



        private void BackCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }


    }
}
