using OpenCvSharp;
namespace ConsoleApp4
{

    public static class Helpers
    {
        public static double GetDiagonalBoxSize(Point[] shape)
        {
            double res = 0.0;
            int dx = 0;
            int dy = 0;
            if (shape.Length > 0)
            {
                dx = shape.Select(p => p.X).Max() - shape.Select(p => p.X).Min();
                dy = shape.Select(p => p.Y).Max() - shape.Select(p => p.Y).Min();
                res = Math.Sqrt(dx * dx + dy * dy);
            }
            return res;
        }

        public static List<Shape> GetShapes(Mat gray)
        {
            gray.FindContours(out var points, out var hier, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            List<Point[]> simple = new List<Point[]>();

            foreach (var shape in points)
            {
                var s = Cv2.ApproxPolyDP(shape, 0.02, true);
                simple.Add(s);
            }


            var list = simple.Select(p => new Shape(p)).ToList();
            return list;
        }

        internal static Shape ExtractMarker(Mat cornerGray)
        {
            var shapes = GetShapes(cornerGray).OrderByDescending(sh => sh.Size).ToList();
            if (shapes.Count > 2)
                throw new Exception("no corners");

            return shapes[1];
        }

        internal static double GetSimilarity(Shape marker, Shape shape)
        {
            throw new NotImplementedException();
        }


        internal static PathElem GetPathElem(Point a, Point b, Point c)
        {
            var pathElem = new PathElem();
            pathElem.Theta = GetAngle(a, b, c);
            int dx = 0;
            int dy = 0;
            dx = b.X - a.X;
            dy = b.Y - a.Y;
            pathElem.DR = Math.Sqrt(dx * dx + dy * dy);
            return pathElem;
        }

        private static float GetAngle(Point a, Point b, Point c)
        {
            var ab = new Point(b.X - a.X, b.Y - a.Y);
            var cb = new Point(b.X - c.X, b.Y - c.Y);

            // dot product  
            float dot = (ab.X * cb.X + ab.Y * cb.Y);

            // length square of both vectors
            float abSqr = ab.X * ab.X + ab.Y * ab.Y;
            float cbSqr = cb.X * cb.X + cb.Y * cb.Y;

            // square of cosine of the needed angle    
            float cosSqr = dot * dot / abSqr / cbSqr;

            // this is a known trigonometric equality:
            // cos(alpha * 2) = [ cos(alpha) ]^2 * 2 - 1
            float cos2 = 2 * cosSqr - 1;

            // Here's the only invocation of the heavy function.
            // It's a good idea to check explicitly if cos2 is within [-1 .. 1] range

            const float pi = 3.141592f;

            float alpha2 =
                (cos2 <= -1) ? pi :
                (cos2 >= 1) ? 0 :
                MathF.Acos(cos2);

            float rslt = alpha2 / 2;

            float rs = rslt * 180 / MathF.PI;

            if (dot < 0)
                rs = 180 - rs;

            // 2. Determine the sign. For this we'll use the Determinant of two vectors.

            float det = (ab.X * cb.Y - ab.Y * cb.Y);
            if (det < 0)
                rs = -rs;
            return rs;
        }
    }

    public class PathElem
    {
        public double DR = 0;
        public double Theta = 0;
    }




}
