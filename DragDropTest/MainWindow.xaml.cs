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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var gNode = new Graph.Node() { Point = new Point(random.Next(0, 500), random.Next(0, 500)) };
                nodes.Add(gNode);
            }

            //nodes.Add(new Graph.Node() { Point = new Point(10, 100) });
            //nodes.Add(new Graph.Node() { Point = new Point(100, 10) });


            for (int i = 0; i < nodes.Count - 1; i++)
            {
                links.Add(new Graph.Link() { StartNode = nodes[i], EndNode = nodes[i + 1] });
            }
            DraggableControlClicked = new Action<DraggableControl>(DCClicked);
        }

        Action<DraggableControl> DraggableControlClicked;
        DraggableControl SelectedDC;

        private  void DCClicked(DraggableControl dc)
        {
            MyGenTextBox2.Text = dc.Name;
            SelectedDC = dc;
        }

        List<Graph.Node> nodes = new List<Graph.Node>();
        List<Graph.Link> links = new List<Graph.Link>();

        



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int counter = 0;
            foreach(var gn in nodes)
            {
                var c = new DraggableControl();
                c.Margin = new Thickness(gn.Point.X, gn.Point.Y, 0, 0);
                c.Clicked = this.DraggableControlClicked;
                c.Name = "DC " + counter.ToString();
                gn.Control = c;
                MyCanvas.Children.Add(c);
                counter++;
            }

            foreach (var link in links)
            {
                var linkControl = new Controls.LinkControl();
                linkControl.InputOffset = (Vector)link.StartNode.Point;
                linkControl.OutputOffset = (Vector)link.EndNode.Point;
                link.LinkControl = linkControl;
                MyBackCanvas.Children.Add(linkControl);
                linkControl.UpdateEnds();
                if (link.StartNode.Point.X < link.EndNode.Point.X)
                {
                    Canvas.SetLeft(linkControl, link.StartNode.Point.X);
                }
                else
                {
                    Canvas.SetLeft(linkControl, link.EndNode.Point.X);
                }

                linkControl.Width = Math.Abs(link.EndNode.Point.X - link.StartNode.Point.X);

                if (link.StartNode.Point.Y < link.EndNode.Point.Y)
                {
                    Canvas.SetTop(linkControl, link.StartNode.Point.Y);
                }
                else
                {
                    Canvas.SetTop(linkControl, link.EndNode.Point.Y);
                }
                linkControl.Height = Math.Abs(link.EndNode.Point.Y - link.StartNode.Point.Y);

                if ((link.EndNode.Point.X - link.StartNode.Point.X) * (link.EndNode.Point.Y - link.StartNode.Point.Y) <= 0)
                {
                    linkControl.SetPathVisible(1);
                }
                else
                {
                    linkControl.SetPathVisible(2);
                }

            }
            //UpdateLinks();
        }


        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = (Vector)e.GetPosition(MyCanvas as IInputElement);
            if (SelectedDC?.IsPressed == true)
            {
                //if (SelectedDC.IsPressed)
                {
                    MyGenTextBox.Text = $"Preview mouse move: Mouse position (canvas): x:{mousePos.X.ToString()}, y:{mousePos.Y.ToString()}";
                    SelectedDC.SetVisualOffset(mousePos - SelectedDC.Delta);
                    UpdateLinks();
                }
            }
        }

        private void UpdateLinks()
        {
            foreach (var link in links)
            {
                var cStart = link.StartNode.Control;
                var cEnd = link.EndNode.Control;
                var startPoint =(Vector) link.StartNode.Control.TransformToAncestor(MyCanvas).Transform(new Point(0, 0));
                var endPoint =(Vector) link.EndNode.Control.TransformToAncestor(MyCanvas).Transform(new Point(0, 0));

                if (startPoint.X < endPoint.X)
                {
                    Canvas.SetLeft(link.LinkControl, startPoint.X);

                }
                else
                {
                    Canvas.SetLeft(link.LinkControl, endPoint.X);
                }

                link.LinkControl.Width = Math.Abs(endPoint.X - startPoint.X);

                if (startPoint.Y < endPoint.Y)
                {
                    Canvas.SetTop(link.LinkControl, startPoint.Y);
                }
                else
                {
                    Canvas.SetTop(link.LinkControl, endPoint.Y);
                }
                link.LinkControl.Height = Math.Abs(endPoint.Y - startPoint.Y);

                if ((endPoint.X - startPoint.X) * (endPoint.Y - startPoint.Y) <= 0)
                {
                    link.LinkControl.SetPathVisible(1);
                }
                else
                {
                    link.LinkControl.SetPathVisible(2);
                }


            }
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
