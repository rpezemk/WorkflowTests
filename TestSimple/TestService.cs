using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TestSimple
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TestService : ITestService
    {
        public void DoWork()
        {
        }

        public string GetSomeData()
        {
            return "Test data";
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TestServiceClient : System.ServiceModel.ClientBase<ITestService>
    {
        public TestServiceClient()
        {

        }
        public TestServiceClient(string endpoint) : base(endpoint)
        {

        }
        public void DoWork()
        {
        }

        public string GetSomeData()
        {
            return base.Channel.GetSomeData();
        }
    }
}
