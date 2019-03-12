namespace Modules.Core.Services.UI.Renderers
{
    using System;

    using Infrastructure.Security;
    using Modules.Core.Contracts.UI.Dto;

    internal sealed class AuthorityRenderer : IDataRenderer<Authority, AuthorityDto>
    {
        public Func<Authority, AuthorityDto> GetSpec() =>
            _ => new AuthorityDto
            {
                Id = _.Id,
                Name = _.DisplayName
            };
    }
}