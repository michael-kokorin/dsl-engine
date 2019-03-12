namespace Modules.UI.Services
{
    using System.Collections.Generic;

    using Modules.Core.Contracts.UI.Dto;

	public interface IAuthorityProvider
    {
        bool IsCan(IEnumerable<string> authorityNames, long? entityId);

	    IEnumerable<ProjectDto> GetProjects(IEnumerable<string> authorityNames);
    }
}