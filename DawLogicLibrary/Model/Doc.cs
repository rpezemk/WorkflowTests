using DawLogicLibrary.DocModels.Primitives;
using System.Collections.Generic;
namespace DawLogicLibrary.DocModels
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
