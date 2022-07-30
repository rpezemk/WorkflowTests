using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace DragDropTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Vector d = new Vector(25, 23);
        public MainWindow()
        {
            InitializeComponent();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var gNode = new Graph.Node() { Point = new Point(random.Next(0, 500), random.Next(0, 500)) };
                nodes.Add(gNode);
            }

            //nodes.Add(new Graph.Node() { Point = new Point(100, 200) });
            //nodes.Add(new Graph.Node() { Point = new Point(0, 0) });


            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Graph.Link newLink = new Graph.Link() { StartNode = nodes[i], EndNode = nodes[i + 1] };
                newLink.StartNode.Point = newLink.StartNode.Point + d;
                links.Add(newLink);
            }
            DraggableControlClicked = new Action<DraggableControl>(DCClicked);
        }

        Action<DraggableControl> DraggableControlClicked;
        Action<DraggableControl> DraggableControlSelected;
        DraggableControl SelectedDC;

        private  void DCClicked(DraggableControl obj)
        {
            MyGenTextBox2.Text = obj.Name;
            SelectedDC = obj;
            SelectedDC.IsSelected = true;
            var counter = 0;

            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                selected.Clear();
            }

            if(!selected.Where(c => c == obj).Any())
                selected.Add(obj);

            foreach (var c in MyCanvas.Children)
            {
                if (c.GetType() != typeof(DraggableControl))
                    continue;

                if (c == obj)
                    continue;
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    (c as DraggableControl).IsSelected = false;
                }
                else
                {
                    
                }
                counter++;
            }

            MyGenTextBox.Text = string.Join(", ", selected.Select(c => c.Name));

        }

        List<Graph.Node> nodes = new List<Graph.Node>();
        List<Graph.Link> links = new List<Graph.Link>();
        List<DraggableControl> selected = new List<DraggableControl>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int counter = 0;
            foreach(var gn in nodes)
            {
                var c = new DraggableControl();
                c.Margin = new Thickness(gn.Point.X, gn.Point.Y, 0, 0);
                c.Clicked = this.DraggableControlClicked;
                c.Selected = this.DraggableControlSelected;
                c.Name = "DC " + counter.ToString();
                gn.Control = c;
                MyCanvas.Children.Add(c);
                counter++;
            }

            foreach (var link in links)
            {
                var linkControl = new Controls.LinkControl();
                var nodeWidth = 0;// link.EndNode.Control.ActualWidth;
                var nodeHeight = 0;// 23;// link.EndNode.Control.Height;
                linkControl.InputOffset = (Vector)link.StartNode.Point; 
                linkControl.OutputOffset = (Vector)link.EndNode.Point;
                link.LinkControl = linkControl;
                MyBackCanvas.Children.Add(linkControl);
                linkControl.UpdateEnds();
                var startPoint = link.StartNode.Point + d;
                if (startPoint.X < link.EndNode.Point.X - nodeWidth)
                {
                    Canvas.SetLeft(linkControl, startPoint.X + nodeWidth);
                }
                else
                {
                    Canvas.SetLeft(linkControl, link.EndNode.Point.X + nodeWidth);
                }

                linkControl.Width = Math.Abs(link.EndNode.Point.X - startPoint.X);

                if (startPoint.Y < link.EndNode.Point.Y - nodeHeight)
                {
                    Canvas.SetTop(linkControl, startPoint.Y + nodeHeight);
                }
                else
                {
                    Canvas.SetTop(linkControl, link.EndNode.Point.Y + nodeHeight);
                }

                linkControl.Height = Math.Abs(link.EndNode.Point.Y - startPoint.Y);

                if (((link.EndNode.Point.X + nodeWidth) - link.StartNode.Point.X) * ((link.EndNode.Point.Y + nodeHeight) - link.StartNode.Point.Y) <= 0)
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
                var ctrlPos = (Vector)SelectedDC.TransformToAncestor(MyCanvas).Transform(new Point(0, 0));
                var delta = ctrlPos - mousePos + 0.5*d;


                if(Math.Abs(delta.X)> 20 || Math.Abs(delta.Y) > 20 || SelectedDC.IsDragged)
                {
                    MyGenTextBox.Text = $"Preview mouse move: Mouse position (canvas): x:{mousePos.X.ToString()}, y:{mousePos.Y.ToString()}";
                    var newOffset = mousePos - SelectedDC.Delta;
                    SelectedDC.SetVisualOffset(newOffset);
                    var updated = nodes.Where(n => n.Control.Equals(SelectedDC)).ToList();
                    foreach(var c in updated)
                    {
                        c.Point = (Point)newOffset;
                    }
                    SelectedDC.IsDragged = true;
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
                var startPoint =(Vector) link.StartNode.Control.TransformToAncestor(MyCanvas).Transform(new Point(0, 0)) + d;
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

        /// <summary>
        /// dodaje link 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) 
        {
            if (selected.Count != 2)
                return;

            var startNode = nodes.Where(n => n.Control == selected[0]).First();
            var endNode = nodes.Where(n => n.Control == selected[1]).First();
            var newLinkControl = new Controls.LinkControl();
            var newLink = new Graph.Link() { StartNode = startNode, EndNode = endNode };
            newLinkControl.InputOffset = (Vector)newLink.StartNode.Point;
            newLinkControl.OutputOffset = (Vector)newLink.EndNode.Point;
            newLink.LinkControl = newLinkControl;
            newLinkControl.UpdateEnds();
            MyBackCanvas.Children.Add(newLinkControl);
            links.Add(newLink);
            UpdateLinks();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                selected.Clear();
                MyGenTextBox.Text = "";
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// Serializuj strukturę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MySerializable mySerializable = new MySerializable();
            mySerializable.SLinks = links.Select(l => new SGraph.SLink() 
            { 
                StartNode = new SGraph.SNode() { Point = l.StartNode.Point, Guid = l.StartNode.Guid },
                EndNode = new SGraph.SNode() { Point = l.EndNode.Point, Guid = l.EndNode.Guid}
            }).ToList();

            mySerializable.SNodes = nodes.Select(n => new SGraph.SNode() { Point = n.Point, Guid = n.Guid }).ToList();


            XmlSerializer x = new XmlSerializer(typeof(MySerializable));
            TextWriter writer = new StreamWriter(@"C:\Users\Michael\Desktop\TestSerialize.xml");
            x.Serialize(writer, mySerializable);
            writer.Dispose();
        }


        /// <summary>
        /// wczytaj strukturę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            XmlSerializer x = new XmlSerializer(typeof(MySerializable));
            TextReader reader = new StreamReader(@"C:\Users\Michael\Desktop\TestSerialize.xml");
            MySerializable result =  x.Deserialize(reader) as MySerializable;
            reader.Dispose();

            MyCanvas.Children.Clear();
            MyBackCanvas.Children.Clear();

            
            nodes = result.SNodes.Select(sn => new Graph.Node() { Guid = sn.Guid, Point = sn.Point }).ToList();
            //nodes.Add(new Graph.Node() { Point = new Point(100, 200) });
            //nodes.Add(new Graph.Node() { Point = new Point(0, 0) });

            links.Clear();
            links = result.SLinks.Select(sl => 
            new Graph.Link(){ 
                    StartNode = nodes.Where(n => n.Guid == sl.StartNode.Guid).First(),
                    EndNode = nodes.Where(n => n.Guid == sl.EndNode.Guid).First()}
            ).ToList();

            int counter = 0;
            foreach (var gn in nodes)
            {
                var c = new DraggableControl();
                c.Margin = new Thickness(gn.Point.X, gn.Point.Y, 0, 0);
                c.Clicked = this.DraggableControlClicked;
                c.Selected = this.DraggableControlSelected;
                c.Name = "DC " + counter.ToString();
                gn.Control = c;
                MyCanvas.Children.Add(c);
                counter++;
            }

            foreach (var link in links)
            {
                var linkControl = new Controls.LinkControl();
                var nodeWidth = 0;// link.EndNode.Control.ActualWidth;
                var nodeHeight = 0;// 23;// link.EndNode.Control.Height;
                linkControl.InputOffset = (Vector)link.StartNode.Point;
                linkControl.OutputOffset = (Vector)link.EndNode.Point;
                link.LinkControl = linkControl;
                MyBackCanvas.Children.Add(linkControl);
                linkControl.UpdateEnds();
                var startPoint = link.StartNode.Point + d;
                if (startPoint.X < link.EndNode.Point.X - nodeWidth)
                {
                    Canvas.SetLeft(linkControl, startPoint.X + nodeWidth);
                }
                else
                {
                    Canvas.SetLeft(linkControl, link.EndNode.Point.X + nodeWidth);
                }

                linkControl.Width = Math.Abs(link.EndNode.Point.X - startPoint.X);

                if (startPoint.Y < link.EndNode.Point.Y - nodeHeight)
                {
                    Canvas.SetTop(linkControl, startPoint.Y + nodeHeight);
                }
                else
                {
                    Canvas.SetTop(linkControl, link.EndNode.Point.Y + nodeHeight);
                }

                linkControl.Height = Math.Abs(link.EndNode.Point.Y - startPoint.Y);

                if (((link.EndNode.Point.X + nodeWidth) - link.StartNode.Point.X) * ((link.EndNode.Point.Y + nodeHeight) - link.StartNode.Point.Y) <= 0)
                {
                    linkControl.SetPathVisible(1);
                }
                else
                {
                    linkControl.SetPathVisible(2);
                }

            }

        }

        /// <summary>
        /// dodaj węzeł
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var gNode = new Graph.Node() { Point = new Point(10, 10) };
            nodes.Add(gNode);
            var c = new DraggableControl();
            c.Margin = new Thickness(gNode.Point.X, gNode.Point.Y, 0, 0);
            c.Clicked = this.DraggableControlClicked;
            c.Selected = this.DraggableControlSelected;
            c.Name = "DC " + nodes.Count();
            gNode.Control = c;
            MyCanvas.Children.Add(c);
        }
    }
}
