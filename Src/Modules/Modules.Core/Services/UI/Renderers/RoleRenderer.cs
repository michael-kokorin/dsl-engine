namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal sealed class RoleRenderer : IDataRenderer<Roles, UserRoleDto>
	{
		public Func<Roles, UserRoleDto> GetSpec() => _ => new UserRoleDto
		{
			DisplayName = _.DisplayName,
			Id = _.Id,
			ProjectId = _.ProjectId,
			Sid = _.Sid
		};
	}
}