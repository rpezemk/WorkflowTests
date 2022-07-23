using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowDemo.DocModels.Primitives;
namespace DawWorkflowDemo.DocModels
{
    public class Doc
    {
        public int GidType = 0;
        public int GidNumer = 0;
        public int GidLp = 0;
        public int GidSubLp = 0;
        public string StrDocNo = "";
        public string SeriesNo = "";
        public NB NB { get; set; } = NB.N;
        public List<Pos> Rows { get; set; } = new List<Pos>();

    }


}
