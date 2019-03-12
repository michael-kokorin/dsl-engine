namespace Infrastructure.Engines
{
    using Microsoft.Practices.Unity;

    internal sealed class WorkflowActionProvider : IWorkflowActionProvider
    {
        private readonly IUnityContainer _container;

        public WorkflowActionProvider(IUnityContainer container)
        {
            _container = container;
        }

        public IWorkflowAction Get(string key) => _container.Resolve<IWorkflowAction>(key);
    }
}