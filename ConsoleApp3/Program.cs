using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            String win1 = "Test Window"; //The name of the window
            CvInvoke.NamedWindow(win1); //Create the window using the specific name
            //Draw "Hello, world." on the image using the specific font
            
            var baseImage = new Image<Gray, float>(@"C:\Users\Przemek.Zaremba\source\repos\WorkflowTests2\ConsoleApp3\test.bmp");
            var maskImage = new Image<Gray, float>(@"C:\Users\Przemek.Zaremba\source\repos\WorkflowTests2\ConsoleApp3\test_01.bmp");
            
            Matrix<float> matrix = new Matrix<float>(maskImage.Mat.Rows, maskImage.Mat.Cols, maskImage.Mat.NumberOfChannels);
            maskImage.Mat.CopyTo(matrix);

            var a = new ConvolutionKernelF(matrix, new Point(0,0));
            var resConv = baseImage.Convolution(a);

            var w = resConv.Width;
            var h = resConv.Height;


            System.Single max = 0.0f;

            foreach(var t in resConv.Data)
            {
                max = t > max ? t : max;
            }

            float coeff = max / 255;

            for(int i = 0; i< resConv.Data.Length; i++)
            {
                var t = resConv.Data[i];
                t = t * coeff;
            }



            foreach (var t in resConv.Data)
            {
                t = t * coeff;
            }

            Console.WriteLine(max);


            CvInvoke.Imshow(win1, resConv); //Show the image
            CvInvoke.WaitKey(0);  //Wait for the key pressing event
            CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed
        }
    }
}
