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
using TestGraphical.Model;

namespace TestGraphical.Controls
{
    /// <summary>
    /// Interaction logic for StepOutput.xaml
    /// </summary>
    public partial class StepOutput : UserControl
    {
        public Guid VisualGuid { get; set; } = Guid.NewGuid();
        public StepOutput()
        {
            InitializeComponent();
        }
        public MLink MLink;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Events.DeleteOutput.Publish(this);
        }



        public Point GetOutputPoint()
        {
            return (this as UIElement).TransformToAncestor(this.GetUIEOfType(typeof(View.MyContentControl))).Transform(new Point(ActualWidth, 0));
        }

    }
}
