using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestGraphical.Controls
{
    public class ObjectArrow
    {
        public double X1 { get; set; } = 0;
        public double Y1 { get; set; } = 0;
        public double X2 { get; set; } = 0;
        public double Y2 { get; set; } = 0;
        public SolidColorBrush Stroke { get; set; }
        public double StrokeThickness { get; set; }

        public void DrawOn(System.Windows.Controls.Panel panel)
        {
            panel.Children.Add(new Line() { X1 = X1, X2 = X2, Y1 = Y1, Y2 = Y2, Stroke = Stroke, StrokeThickness = StrokeThickness });
        }

    }




}
