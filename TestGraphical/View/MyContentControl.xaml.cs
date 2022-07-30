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
            Events.AddStepToCanvasEvt.Subscribe(TestABC);
            Events.RefreshControlsEvt.Subscribe(TestSub);
        }
        private int counter = 0;
        private void TestABC(VM_Step vm_step)
        {
            Controls.StepControl stepControl = new Controls.StepControl();
            vm_step.XOffset = counter;
            vm_step.YOffset = counter;
            counter += 10;
            stepControl.DataContext = vm_step;
            stepControl.VisualGuid = vm_step.VisualGuid;
            MyCanvas.Children.Add(stepControl);
        }

        private DelegateCommand canvasClickedCmd;
        public DelegateCommand CanvasClickedCmd =>
            canvasClickedCmd ?? (canvasClickedCmd = new DelegateCommand(CanvasClicked));


        private DelegateCommand canvasUnclickedCmd;
        public DelegateCommand CanvasUnclickedCmd =>
            canvasUnclickedCmd ?? (canvasUnclickedCmd = new DelegateCommand(CanvasUnclicked));

        private void CanvasClicked()
        {

        }
        void CanvasUnclicked()
        {

        }



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



        private void TestSub()
        {

        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void MyCanvas_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
