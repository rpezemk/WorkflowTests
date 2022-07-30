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

namespace DragDropTest
{
    /// <summary>
    /// Interaction logic for DraggableControl.xaml
    /// </summary>
    public partial class DraggableControl : UserControl
    {
        public DraggableControl()
        {
            InitializeComponent();
        }


        public bool IsPressed = false;
        public bool IsDragged = false;
        public bool IsSelected = false;
        public Vector Delta = new Vector(0, 0);
        public Action<DraggableControl> Clicked = null;
        public Action<DraggableControl> Selected = null;
        public string Name = "";

        public void SetVisualOffset(Vector offset)
        {
            this.VisualOffset = offset;
        }


        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = true;
            IsSelected = true;
            Vector mousePos = (Vector)e.GetPosition(this.Parent as IInputElement);
            Vector thisPos = this.VisualOffset;
            Delta = mousePos - thisPos;
            Background = new SolidColorBrush(Colors.DarkGray);
            Clicked?.Invoke(this);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsPressed = false;
            IsDragged = false;
            Background = new SolidColorBrush(Colors.White);

        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (IsPressed)
            //{
            //    Vector mousePos = (Vector)e.GetPosition(this.Parent as IInputElement);
            //    this.VisualOffset = mousePos - Delta;
            //}
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MyTextBlock0.Text = Name;
        }
    }
}
