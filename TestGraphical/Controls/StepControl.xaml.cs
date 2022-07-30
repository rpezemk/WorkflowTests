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
        public Guid VisualGuid { get; set; }


        public StepControl()
        {
            InitializeComponent();
        }

        private void MyStepControl_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MyStepControl_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
