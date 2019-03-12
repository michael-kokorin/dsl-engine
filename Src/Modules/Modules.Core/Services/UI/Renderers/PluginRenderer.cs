namespace Modules.Core.Services.UI.Renderers
{
    using System;

    using Modules.Core.Contracts.UI.Dto.UserSettings;
    using Repository.Context;

    internal sealed class PluginRenderer : IDataRenderer<Plugins, PluginDto>
    {
        public Func<Plugins, PluginDto> GetSpec() => _ => new PluginDto
        {
            DisplayName = _.DisplayName,
            Id = _.Id,
            Type = (PluginTypeDto) _.Type
        };
    }
}