using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowDemo.DocModels.Primitives;

namespace DawWorkflowDemo.SomeApi
{
    public static class API
    {
        public static DocModels.Zam GetZam(string docStrNo)
        {
            var seriesNo = docStrNo.Split('\\').Skip(1).LastOrDefault();
            DocModels.Zam zam;
            if (string.IsNullOrEmpty(seriesNo))
            {
                zam = new DocModels.Zam() { GidType = 960, GidNumer = 234646346, NB = NB.N, SeriesNo = seriesNo };
            }
            else if (docStrNo == "ErrorDoc")
            {
                zam = null;
            }
            else
            {
                zam = new DocModels.Zam() { GidType = 960, GidNumer = 54363, NB = NB.B, SeriesNo = "ABC1" };
            }
            return zam;
        }

        public static DocModels.Doc GetDoc(string docStrNo)
        {
            var seriesNo = docStrNo.Split('\\').Skip(1).LastOrDefault();
            DocModels.Doc zam;
            if (string.IsNullOrEmpty(seriesNo))
            {
                zam = new DocModels.Doc() { GidType = 960, GidNumer = 234646346, NB = NB.N, SeriesNo = seriesNo };
            }
            else if(docStrNo == "ErrorDoc")
            {
                zam = null;
            }
            else
            {
                zam = new DocModels.Doc() { GidType = 960, GidNumer = 54363, NB = NB.B, SeriesNo = "ABC1" };
            }
            return zam;
        } 


        public static int CreateDoc()
        {
            Random r = new Random();
            var a = r.Next(0, 5);
            var res = 0;
            switch(a)
            {
                case 1: res = 2005;
                    break;
                case 2: res = 2036;
                    break;
                case 3: res = 2037;
                    break;
                default: res = 0;
                    break;
            }
            return res;
        }


    }
}
