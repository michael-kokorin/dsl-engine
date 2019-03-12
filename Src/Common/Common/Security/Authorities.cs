namespace Common.Security
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides authorities in the application.
	/// </summary>
	/// <seealso cref="Common.Security.IAuthority"/>
	[UsedImplicitly]
	public sealed class Authorities: IAuthority
	{
		/// <summary>
		///   Gets all authorities.
		/// </summary>
		/// <returns>
		///   Collection of authorities.
		/// </returns>
		public IEnumerable<string> All() => new UI().All();

		// ReSharper disable once InconsistentNaming
		/// <summary>
		///   Provides authorities related to UI.
		/// </summary>
		/// <seealso cref="Common.Security.IAuthority"/>
		public sealed class UI: IAuthority
		{
			/// <summary>
			///   Gets all UI authorities.
			/// </summary>
			/// <returns>
			///   Collection of UI authorities.
			/// </returns>
			public IEnumerable<string> All() =>
				new Project()
					.All()
					.Union(new PersonalCabinet().All())
					.Union(new Administration().All())
					.Union(new Reports().All())
					.Union(new Queries().All());

			/// <summary>
			///   Provides the UI administration authorities.
			/// </summary>
			/// <seealso cref="Common.Security.IAuthority"/>
			public sealed class Administration: IAuthority
			{
				/// <summary>
				///   The edit administration panel authority.
				/// </summary>
				public const string Edit = "edit_administration_panel";

				/// <summary>
				///   The view administration panel authority.
				/// </summary>
				public const string View = "view_administraion_panel";

				/// <summary>
				///   Gets all authorities.
				/// </summary>
				/// <returns>
				///   Collection of authorities.
				/// </returns>
				public IEnumerable<string> All() =>
					new[]
					{
						Edit,
						View
					};
			}

			/// <summary>
			///   Provides personal cabinet authorities.
			/// </summary>
			/// <seealso cref="Common.Security.IAuthority"/>
			public sealed class PersonalCabinet: IAuthority
			{
				/// <summary>
				///   The edit plugin settings authority.
				/// </summary>
				public const string EditPluginSettings = "edit_personal_cabinet_plugin_settings";

				/// <summary>
				///   Gets all authorities.
				/// </summary>
				/// <returns>
				///   Collection of authorities.
				/// </returns>
				public IEnumerable<string> All() =>
					new[]
					{
						EditPluginSettings
					};
			}

			/// <summary>
			///   Provides the project authorities.
			/// </summary>
			/// <seealso cref="Common.Security.IAuthority"/>
			public sealed class Project: IAuthority
			{
				/// <summary>
				///   Gets all authorities.
				/// </summary>
				/// <returns>
				///   Collection of authorities.
				/// </returns>
				public IEnumerable<string> All() =>
					new Settings()
						.All()
						.Union(new Tasks().All())
						.Union(new ProjectsList().All());

				/// <summary>
				///   Provides the project list authorities.
				/// </summary>
				/// <seealso cref="Common.Security.IAuthority"/>
				public sealed class ProjectsList: IAuthority
				{
					/// <summary>
					///   The create project project authority.
					/// </summary>
					public const string Create = "create_project";

					/// <summary>
					///   The view projects authority.
					/// </summary>
					public const string View = "view_projects";

					/// <summary>
					///   Gets all authorities.
					/// </summary>
					/// <returns>
					///   Collection of authorities.
					/// </returns>
					public IEnumerable<string> All() => new[] {Create, View}
						.Union(new Stat().All());

					/// <summary>
					///   Provides project statistics authorities.
					/// </summary>
					/// <seealso cref="Common.Security.IAuthority"/>
					public sealed class Stat: IAuthority
					{
						/// <summary>
						///   The view project health authority.
						/// </summary>
						public const string Health = "view_project_stat_health";

						/// <summary>
						///   The view project metrics authority.
						/// </summary>
						public const string Metrics = "view_project_stat_metrics";

						/// <summary>
						///   The view project vulnerabilities authority.
						/// </summary>
						public const string Vulnerabilities = "view_project_stat_vulnerabilities";

						/// <summary>
						///   Gets all authorities.
						/// </summary>
						/// <returns>
						///   Collection of authorities.
						/// </returns>
						public IEnumerable<string> All() =>
							new[]
							{
								Health,
								Metrics,
								Vulnerabilities
							};
					}
				}

				/// <summary>
				///   Provides project settings authorities.
				/// </summary>
				/// <seealso cref="Common.Security.IAuthority"/>
				public sealed class Settings: IAuthority
				{
					/// <summary>
					///   The edit project settings authority.
					/// </summary>
					public const string Edit = "edit_project_settings_base";

					/// <summary>
					///   The edit access control authority.
					/// </summary>
					public const string EditAccessControl = "edit_project_settings_access_control";

					/// <summary>
					///   The edit issue tracker authority.
					/// </summary>
					public const string EditIssueTracker = "edit_project_settings_issue_tracker";

					/// <summary>
					///   The edit notifications authority.
					/// </summary>
					public const string EditNotifications = "edit_project_settings_notification_rules";

					/// <summary>
					///   The edit scan core authority.
					/// </summary>
					public const string EditScanCore = "edit_project_settings_scan_core";

					/// <summary>
					///   The edit SDL policy authority.
					/// </summary>
					public const string EditSdlPolicy = "edit_project_settings_sdl_policy";

					/// <summary>
					///   The edit version control authority.
					/// </summary>
					public const string EditVersionControl = "edit_project_settings_version_control";

					/// <summary>
					///   The view project settings authority.
					/// </summary>
					public const string View = "view_project_settings_base";

					/// <summary>
					///   The view access control authority.
					/// </summary>
					public const string ViewAccessControl = "view_project_settings_access_control";

					/// <summary>
					///   The view issue tracker authority.
					/// </summary>
					public const string ViewIssueTracker = "view_project_settings_issue_tracker";

					/// <summary>
					///   The view notifications authority.
					/// </summary>
					public const string ViewNotifications = "view_project_settings_notification_rules";

					/// <summary>
					///   The view scan core authority.
					/// </summary>
					public const string ViewScanCore = "view_project_settings_scan_core";

					/// <summary>
					///   The view SDL policy authority.
					/// </summary>
					public const string ViewSdlPolicy = "view_project_settings_sdl_policy";

					/// <summary>
					///   The view version control authority.
					/// </summary>
					public const string ViewVersionControl = "view_project_settings_version_control";

					/// <summary>
					///   Gets all authorities.
					/// </summary>
					/// <returns>
					///   Collection of authorities.
					/// </returns>
					public IEnumerable<string> All() =>
						new[]
						{
							Edit,
							EditAccessControl,
							EditIssueTracker,
							EditNotifications,
							EditScanCore,
							EditSdlPolicy,
							EditVersionControl,
							View,
							ViewAccessControl,
							ViewIssueTracker,
							ViewNotifications,
							ViewSdlPolicy,
							ViewScanCore,
							ViewVersionControl
						};
				}

				/// <summary>
				///   Provides tasks authorities.
				/// </summary>
				/// <seealso cref="Common.Security.IAuthority"/>
				public sealed class Tasks: IAuthority
				{
					/// <summary>
					///   The create new task authority.
					/// </summary>
					public const string CreateNewTask = "create_new_task";

					/// <summary>
					///   The stop task executing authority.
					/// </summary>
					public const string StopTaskExecuting = "stop_task_executing";

					/// <summary>
					///   The view task authority.
					/// </summary>
					public const string View = "view_task";

					/// <summary>
					///   The view results authority.
					/// </summary>
					public const string ViewResults = "view_results";

					/// <summary>
					///   Gets all authorities.
					/// </summary>
					/// <returns>
					///   Collection of authorities.
					/// </returns>
					public IEnumerable<string> All() =>
						new[]
						{
							CreateNewTask,
							StopTaskExecuting,
							View,
							ViewResults
						};
				}
			}

			/// <summary>
			///   Provides queries authorities.
			/// </summary>
			/// <seealso cref="Common.Security.IAuthority"/>
			public sealed class Queries: IAuthority
			{
				/// <summary>
				///   The create query authority.
				/// </summary>
				public const string CreateQuery = "create_query";

				/// <summary>
				///   The edit query authority.
				/// </summary>
				public const string EditQuery = "edit_query";

				/// <summary>
				///   The edit query all authority.
				/// </summary>
				public const string EditQueryAll = "edit_query_all";

				/// <summary>
				///   The view queries all authority.
				/// </summary>
				public const string ViewQueriesAll = "view_queries_all";

				/// <summary>
				///   The view query authority.
				/// </summary>
				public const string ViewQuery = "view_query";

				/// <summary>
				///   Gets all authorities.
				/// </summary>
				/// <returns>
				///   Collection of authorities.
				/// </returns>
				public IEnumerable<string> All() =>
					new[]
					{
						CreateQuery,
						EditQuery,
						EditQueryAll,
						ViewQuery,
						ViewQueriesAll
					};
			}

			/// <summary>
			///   Provides report authorities.
			/// </summary>
			/// <seealso cref="Common.Security.IAuthority"/>
			public sealed class Reports: IAuthority
			{
				public const string Create = "create_report";

				public const string Edit = "edit_report";

				public const string Run = "run_reports";

				public const string View = "run_reports";

				public IEnumerable<string> All() => new[] { Create, Edit, Run};
			}
		}
	}
}