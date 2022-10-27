using OpenCvSharp;
namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string inputFileName = @"C:\Users\Przemek.Zaremba\source\repos\WorkflowTests2\ConsoleApp3\test.bmp";
            string outputFileName = $"{Path.GetFileNameWithoutExtension(inputFileName)}-gray.jpg";
            var image = new Mat(inputFileName);
            var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);
            gray.SaveImage(outputFileName);
            Cv2.ImShow("test", gray);
            Cv2.WaitKeyEx();
            Mat[] mats = new List<Mat>().ToArray();
           
            gray.FindContours(out var points, out var hier, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            foreach (var shape in points)
            {
                foreach(var )
            }
           
        }
    }
}
