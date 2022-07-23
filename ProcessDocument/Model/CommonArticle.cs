using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDocument.Model
{
    internal class CommonArticle
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int XLID { get; set; }
        public int TwrNumer { get; set; }
        public string SKU { get; set; }
        public string Prefix { get; set; }
        public string TwrCode { get; set; }
        public decimal MinStock { get; set; }
        public string CompanyName { get; set; }
        public bool DisableSelling { get; set; }
    }

    internal class CommonArticleMapping
    {
        public int ID { get; set; }
        public int XLID { get; set; }
        public int CommonArticleID { get; set; }
        public int TwrNumer { get; set; }
        public string TwrCode { get; set; }
    }

    internal class CommonArticleDoc
    {
        public CommonArticleDocHead Head { get; set; } = new CommonArticleDocHead();
        public List<CommonArticleDocPos> Pos { get; set; } = new List<CommonArticleDocPos>();

        internal class CommonArticleDocHead
        {
            public int ID { get; set; }
            public int SourceCompanyID { get; set; }
            public int DestCompanyID { get; set; }
            public int SourceXLID { get; set; }
            public int DestXLID { get; set; }
            public int RelatedID { get; set; }
            public int GIDTyp { get; set; }
            public int GIDNumer { get; set; }
            public string DocNumber { get; set; } = string.Empty;
            public int SourceGIDTyp { get; set; }
            public int SourceGIDNumer { get; set; }
            public string SourceDocNumber { get; set; }
            public string Property1 { get; set; } = string.Empty;
            public string Property2 { get; set; } = string.Empty;
            public bool Export { get; set; }
        }

        internal class CommonArticleDocPos
        {
            public int ID { get; set; }
            public int HeadID { get; set; }
            public int Lp { get; set; }
            public int CommonTwrNumer { get; set; }
            public string CommonTwrCode { get; set; } = string.Empty;
            public int MappedTwrNumer { get; set; }
            public string MappedTwrCode { get; set; } = string.Empty;
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
