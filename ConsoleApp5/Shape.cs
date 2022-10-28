using OpenCvSharp;

namespace ConsoleApp4
{
    public class Shape
    {
        public Shape()
        {

        }

        public Shape(Point[] points)
        {
            Points = points;
        }

        private double size = 0;

        public Point[] Points = (new List<Point>()).ToArray();
        public double Size
        {
            get 
            { 
                if(size != 0) return size; 
                size = Helpers.GetDiagonalBoxSize(Points);
                return size;
            }
        }

    }
}
