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

namespace DragDropTest.Controls
{
    /// <summary>
    /// Interaction logic for LinkControl.xaml
    /// </summary>
    public partial class LinkControl : UserControl
    {
        public LinkControl()
        {
            InitializeComponent();
        }

        public Vector InputOffset;
        public Vector OutputOffset;

        public void SetPathVisible(int p)
        {
            if(p == 1)
            {
                Path1.Visibility = Visibility.Visible;
                Path2.Visibility = Visibility.Hidden;
            }
            if (p == 2)
            {
                Path1.Visibility = Visibility.Hidden;
                Path2.Visibility = Visibility.Visible;
            }
        }

        public void UpdateEnds()
        {
            double X1, X2, Y1, Y2;
            if(InputOffset.X < OutputOffset.X)
            {
                X1 = InputOffset.X;
                X2 = OutputOffset.X;
            }
            else
            {
                X2 = InputOffset.X;
                X1 = OutputOffset.X;
            }

            if(InputOffset.Y < OutputOffset.Y)
            {
                Y1 = InputOffset.Y;
                Y2 = OutputOffset.Y;
            }
            else
            {
                Y2 = InputOffset.Y;
                Y1 = OutputOffset.Y;
            }

            //this.VisualOffset = new Vector(X1, Y1);
            //this.Width = X2 - X1;
            //this.Height = Y2 - Y1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
