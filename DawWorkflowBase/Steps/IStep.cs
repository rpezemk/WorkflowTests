using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DawWorkflowBase.Context;
using DawWorkflowBase.Steps;
using DawWorkflowBase.Context;

namespace DawWorkflowBase.Steps
{
    public interface IStep 
    {
        string GetName();
        void AcceptContext(IContext parentContext);
        bool CheckIfEndPoint();
        void RunStep(IContext context);
        void SetEndPoint(bool isEndPoint);
        IContext GetContext();
        List<Links.ILinkInstance> GetLinks();
        string GetContextTypeName();
        string ToString();
    }

    public class Dupa
    {
        public string Nazwa;
        private string nazwa2;

        public string Nazwa2
        {
            get { return nazwa2; }
            set { nazwa2 = value; }
        }
    }

    public static class Helpers
    {
        public static void TestMethod()
        {
            Dupa dupa1 = new Dupa();


            var t = dupa1.GetType();


            foreach(var p in t.GetProperties())
            {

            }

            foreach (var f in t.GetFields().Where(f => f.IsPublic == false))
            {

            }

        }
    }

    
}
