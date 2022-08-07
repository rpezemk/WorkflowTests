using System;
using System.Collections.Generic;
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

namespace TestGraphical.Controls
{
    /// <summary>
    /// Interaction logic for StepControl.xaml
    /// </summary>
    public partial class StepControl : UserControl
    {
        public Point MousePos;

        public Guid VisualGuid { get; set; }
        public Point ClickPos { get; set; }
        public bool IsClicked { get; set; }
        public void SetPosition(Point p)
        {
            this.Margin = new Thickness(p.X, p.Y, 0, 0);
            //this.VisualOffset = (Vector)p;
        }

        public StepControl()
        {
            InitializeComponent();
            Events.DeleteOutput.Subscribe(DeleteOutput);
        }

        private void DeleteOutput(StepOutput obj)
        {
            if (obj == null)
                return;

            if (OutputsPanel.Children.Contains(obj))
            {
                OutputsPanel.Children.Remove(obj);
            }
        }

        private void MyStepControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsClicked = true;
            ClickPos = e.GetPosition(this as IInputElement);
            Events.ControlClicked.Publish(sender as StepControl);
        }

        private void MyStepControl_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Events.ControlUnClicked.Publish(sender as StepControl);
            IsClicked = false;
        }

        private void MyStepControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (IsClicked)
            {
                MousePos = e.GetPosition(this.Parent as UIElement);
                this.Margin = new Thickness(MousePos.X - ClickPos.X, MousePos.Y - ClickPos.Y, 0, 0);
                Events.ConnectExperimental.Publish();
                //this.VisualOffset = MousePos - ClickPos;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OutputsPanel.Children.Add(new StepOutput());
            this.Margin = new Thickness(MousePos.X - ClickPos.X, MousePos.Y - ClickPos.Y, 0, 0);
        }

        private void MyStepControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
