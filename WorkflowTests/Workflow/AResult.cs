using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTests
{


    public interface IWorkflowHost
    {
        void RegisterWorkflow<WorkFlowT, DataT>() where WorkFlowT : IWorkflow<DataT>;
        //void StartWorkflows
    }



    public interface IWorkflow<DataT>
    {
        void Start();
    }


    public class DocProcessWorkflow<DocType> : IWorkflow<DocType> where DocType : Doc
    {
        public void Start()
        {
            throw new NotImplementedException();
        }
    }


    public class DocWorkflowHost : IWorkflowHost
    {


        public void RegisterWorkflow<T1, T2>() where T1 : IWorkflow<T2>
        {
            throw new NotImplementedException();
        }
    }



    public interface INode
    {
        IResult Run();
        void LogError(IResult result);
    }


    public abstract class ANode : INode
    {
        public IResult Run()
        {
            throw new NotImplementedException();
        }
        public void LogError(IResult result)
        {
            throw new NotImplementedException();
        }

    }





    public interface IResult
    {
        
    }

    

    public class APIDocAddResult : IResult
    {

    }

    public class APIDocOpenResult : IResult
    {

    }




    public class Step
    {

    }




}
