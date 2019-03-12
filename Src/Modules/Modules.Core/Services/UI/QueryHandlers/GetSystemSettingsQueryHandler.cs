namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Query;
	using Common.Security;
	using Common.Settings;
	using Infrastructure;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Mail;
	using Infrastructure.Plugins;
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class GetSystemSettingsQueryHandler : IDataQueryHandler<GetSystemSettingsQuery, SettingsDto>
	{
		private readonly IConfigManager _configManager;

		private readonly IConfigurationProvider _configurationProvider;

		private readonly IMailConnectionParametersProvider _mailConnectionParametersProvider;

		private readonly IPluginProvider _pluginProvider;

		private readonly IScanAgentRepository _scanAgentRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		public GetSystemSettingsQueryHandler(
			IConfigManager configManager,
			IConfigurationProvider configurationProvider,
			IMailConnectionParametersProvider mailConnectionParametersProvider,
			IPluginProvider pluginProvider,
			IScanAgentRepository scanAgentRepository,
			IUserAuthorityValidator userAuthorityValidator,
			IUserPrincipal userPrincipal)
		{
			_configManager = configManager;
			_configurationProvider = configurationProvider;
			_mailConnectionParametersProvider = mailConnectionParametersProvider;
			_pluginProvider = pluginProvider;
			_scanAgentRepository = scanAgentRepository;
			_userAuthorityValidator = userAuthorityValidator;
			_userPrincipal = userPrincipal;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public SettingsDto Execute(GetSystemSettingsQuery dataQuery)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[] {Authorities.UI.Administration.View},
				null))
				throw new UnauthorizedAccessException();

			var mailConnectionParameters = _mailConnectionParametersProvider.TryGet();

			var settings = new SettingsDto
			{
				ActiveDirectorySettings = new ActiveDirectorySettingsDto
				{
					RootGroupPath =
						_configurationProvider.GetValue(ConfigurationKeys.AppSettings.ActiveDirectoryRootGroup)
				},
				DatabaseSettings = new DatabaseSettingsDto
				{
					ConnectionString = _configManager.GetConnectionString()
				},
				FileStorageSettings = new FileStorageSettingsDto
				{
					TempDirPath = _configurationProvider.GetValue(ConfigurationKeys.AppSettings.TempDirPath)
				},
				ScanAgentSettings = new ScanAgentSettingsDto
				{
					ScanAgents = new TableRenderer().Render(
						_scanAgentRepository.Query()
							.Select(_ => new QueryResultItem
							{
								EntityId = _.Id,
								Value = new
								{
									_.Uid,
									_.Machine,
									Version = _.AssemblyVersion
								}
							}))
				},
				PluginSettings = new PluginSettingsDto
				{
					Plugins = new TableRenderer().Render(
						_pluginProvider.Get(_ => true)
							.Select(_ => new QueryResultItem
							{
								EntityId = _.Id,
								Value = new
								{
									Name = _.DisplayName,
									Type = ((PluginType) _.Type).ToString()
								}
							}))
				},
				NotificationSettings = new NotificationSettingsDto
				{
					MailServerHost = mailConnectionParameters.Host,
					IsSslEnabled = mailConnectionParameters.IsSslEnabled,
					MailBox = mailConnectionParameters.Mailbox,
					MainServerPort = mailConnectionParameters.Port,
					Password = mailConnectionParameters.Password,
					UserName = mailConnectionParameters.Username
				}
			};

			return settings;
		}
	}
}