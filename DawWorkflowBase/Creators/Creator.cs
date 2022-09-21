using DawWorkflowBase.Context;
using DawWorkflowBase.Links;
using DawWorkflowBase.Steps;
using System;
using System.Linq;
using System.Reflection;

namespace DawWorkflowBase.Creators
{
    public class Creator
    {
        public IStep GenerateStep(Type stepType, string stepName)
        {
            IStep resultStep = null;

            if (stepType.GetInterface(nameof(IStep)) == null)
                throw new Exception("Wrong step type!!!");
            return (IStep)Activator.CreateInstance(stepType);
        }

        public IStep GenerateStep(string stepTypeName, string contextTypeName)
        {
            Type stepDefType = typeof(StepDef<>);

            //var contextType = typeof(TestContext);
            var contextType = typeof(TestContext);// Assembly.GetCallingAssembly().GetTypes().Where(t => t.GetInterfaces().Where(i => i == typeof(IContext)).Any()).Where(t => t.Name.ToLower().Contains(contextTypeName.ToLower())).First();

            var args = new[] { contextType };

            var stepDefCertainType = stepDefType.MakeGenericType(args);

            var stepDefInstance = Activator.CreateInstance(stepDefCertainType);


            Type stepType = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Where(i => i == typeof(IStep)).Any()).Where(t => t.Name.ToLower().Contains(stepTypeName.ToLower())).First();

            Type stepCertainType = stepType.MakeGenericType(new[] { contextType });

            var constructors = stepCertainType.GetConstructors()[0].GetMethodBody();
            var res = Activator.CreateInstance(stepCertainType, new[] { stepDefInstance });
            return (IStep)res;
        }


    }

    public class TestContext : IContext
    {
        public string GetName()
        {
            return this.GetType().Name;
        }
    }
}
