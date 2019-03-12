namespace Modules.UI.NodeProviders
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using MvcSiteMapProvider;

    using Common.Container;
    using Modules.Core.Contracts.UI;

    [UsedImplicitly]
    public sealed class ProjectNodeProvider : DynamicNodeProviderBase
    {
        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <returns>
        /// A dynamic node collection.
        /// </returns>
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var apiService = IoC.Resolve<IApiService>();

            var projects = apiService.GetProjectsByUser();

            if (projects.Length == 0)
                return new DynamicNode[0];

            var nodes = new List<DynamicNode>();

            foreach (var project in projects)
            {
                var newNode = new DynamicNode
                {
                    Key = "project_" + project.Id,
                    Action = "GetByProject",
                    Controller = "Task",
                    Title = project.Name ?? project.Alias,
                    ParentKey = "Projects"
                };

                newNode.RouteValues.Add("projectId", project.Id);

                nodes.Add(newNode);
            }

            return nodes;
        }
    }
}