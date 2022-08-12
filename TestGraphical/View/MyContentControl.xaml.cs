using Prism.Commands;
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
            Events.ControlClicked.Subscribe(ChildControlClicked);
            Events.ControlUnClicked.Subscribe(ChildControlUnClicked);
            Events.RefreshLinesEvent.Subscribe(RefreshConnections);
            Events.RefreshWorkflow.Subscribe(RefreshWorkflow);
            Events.TestEvt.Subscribe(RefreshConnections);
        }


        private void UpdateDimensions()
        {
            return;
            var x = 0.0;
            var y = 0.0;
            var margin = 100.0;
            var points = MyCanvas.Children.OfType<StepControl>().Select(sc => new Point() { X = sc.Margin.Left, Y = sc.Margin.Top + sc.ActualHeight}).ToList();
            x = Math.Max(MyContentGrid.ActualWidth, points.Select(p => p.X).Max());
            y = points.Select(p => p.Y).Max();
            MyContentGrid.Width = x;
            MyContentGrid.Height = y;
        }

        private void RefreshWorkflow()
        {
            if (DataContext == null)
                return;
            if (DataContext.GetType() != typeof(VM_Workflow))
                return;
            var vm = DataContext as VM_Workflow;
            if (vm.StepVMs == null)
                return;
            foreach (var stepVM in vm.StepVMs)
            {
                AddControl(stepVM);
            }
            UpdateLayout();
            RefreshConnections();

        }

        private void ChildControlUnClicked(StepControl obj)
        {
            isControlClicked = false;
        }

        private void RefreshConnections()
        {
            var mLinks = MyCanvas.Children.OfType<StepControl>().Where(sc => sc.DataContext != null).Where(sc => (sc.DataContext as VM_Step) != null)
                .Select(sc => (sc.DataContext as VM_Step).MStep ?? new Model.MStep("added", "step", 10, 10))
                .SelectMany(mstep => mstep.Outputs).ToList();
            var count = mLinks.Count();

            var stepControls = MyCanvas.Children.OfType<StepControl>().ToList();
            var width = 50.0;
            if (stepControls.Count > 0)
                width = stepControls[0].ActualWidth;

            var outputs = stepControls.SelectMany(sc => sc.OutputsPanel.Children.OfType<StepOutput>()).ToList();




            var OArrows = mLinks.Select(link => new ObjectArrow()
            {
                X1 = outputs.Where(o => o.MLink == link).First().GetOutputPoint().X,
                Y1 = outputs.Where(o => o.MLink == link).First().GetOutputPoint().Y + 10,
                X2 = stepControls.Where(sc => link.OutputStep == sc.MStep).First().GetOutputPoint().X,
                Y2 = stepControls.Where(sc => link.OutputStep == sc.MStep).First().GetOutputPoint().Y
            });

            BackCanvas.Children.Clear();
            foreach (var arrow in OArrows)
            {
                arrow.StrokeThickness = 2.0;
                arrow.Stroke = new SolidColorBrush(Colors.Black);
                arrow.DrawOn(BackCanvas);//.Children.Add(l);
            }
            UpdateDimensions();
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
            if (obj.MStep != null && isControlClicked)
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                    Events.ClearSelectionEvent.Publish();

                Events.StepSelectedEvent.Publish(obj.MStep);
            }

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
            if (SelectedControl != null)
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
                    if (SelectedControl != null && isControlClicked)
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
            foreach (var ctl in ordered)
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
            //vm_step.Name = vm_step.;
            counter += 10;
            stepControl.OutputsPanel.Children.Clear();
            if (vm_step.MStep != null)
            {
                if (vm_step.MStep.Outputs != null)
                {
                    foreach (var ms in vm_step.MStep.Outputs)
                    {
                        stepControl.OutputsPanel.Children.Add(new StepOutput() { MLink = ms });
                    }
                    stepControl.MLinkOutputs = vm_step.MStep.Outputs;
                }
                stepControl.Margin = new Thickness(vm_step.MStep.XOffset, vm_step.MStep.YOffset, 0, 0);
            }

            stepControl.MStep = vm_step.MStep;
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
