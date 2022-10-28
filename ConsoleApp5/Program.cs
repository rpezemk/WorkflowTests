using OpenCvSharp;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp4
{
    internal partial class Program
    {
        static void Main(string[] args)
        {

            string inputFileName = @"C:\Users\Przemek.Zaremba\source\repos\WorkflowTests2\ConsoleApp5\input.bmp";
            string outputFileName = $"{Path.GetFileNameWithoutExtension(inputFileName)}-gray.jpg";
            var image = new Mat(inputFileName);
            var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY).Threshold(190, 255, ThresholdTypes.Binary);
            //Cv2.ImShow("test", gray);
            //Cv2.WaitKeyEx();
            Mat[] mats = new List<Mat>().ToArray();


            string cornerFileName = @"C:\Users\Przemek.Zaremba\source\repos\WorkflowTests2\ConsoleApp5\corner.bmp";
            var cornerGray = new Mat(cornerFileName).CvtColor(ColorConversionCodes.BGR2GRAY).Threshold(190, 255, ThresholdTypes.Binary);
            Cv2.ImShow("test", gray);
            Cv2.WaitKeyEx();
            Cv2.ImShow("test", cornerGray);
            Cv2.WaitKeyEx();
            List<Shape> inputShapes = Helpers.GetShapes(gray);

            Shape marker = Helpers.ExtractMarker(cornerGray);

            foreach (Shape shape in inputShapes)
            {
                double res = Helpers.GetSimilarity(marker, shape);
            }

            Console.WriteLine("test");

        }

        
    }
}
