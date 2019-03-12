namespace Modules.Core.Services.UI.Renderers
{
    using System;

    using Modules.Core.Contracts.UI.Dto;
    using Repository.Context;

    internal sealed class SdlRuleRenderer : IDataRenderer<PolicyRules, SdlRuleDto>
    {
        public Func<PolicyRules, SdlRuleDto> GetSpec() => _ =>
            new SdlRuleDto
            {
                DisplayName = _.Name,
                Id = _.Id,
                Query = _.Query
            };
    }
}