namespace PT.Sdl.Infrastructure.Plugins.Agent.Client.Contracts
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Threading;
	using System.Threading.Tasks;

	using JetBrains.Annotations;

	using PT.Sdl.Infrastructure.Plugins.Agent.Contracts;
	using PT.Sdl.Infrastructure.Plugins.Common.Contracts;

	/// <summary>
	///   Provides methods for agent plugin.
	/// </summary>
	[InheritedExport(typeof(IAgentClientPlugin))]
	public interface IAgentClientPlugin: IPlugin
	{
		/// <summary>
		///   Gets the plugin key.
		/// </summary>
		/// <value>
		///   The plugin key.
		/// </value>
		[NotNull]
		string Key { get; }

		/// <summary>
		///   Initializes this instance.
		/// </summary>
		void Initialize();

		/// <summary>
		///   Runs plugin for the specified codeLocation.
		/// </summary>
		/// <param name="rootFolder">The root folder.</param>
		/// <param name="codeLocation">The codeLocation.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="localParameters">The local parameters.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///   Execution result.
		/// </returns>
		[NotNull]
		Task<ScanTaskResultDto> Run(
			string rootFolder,
			[NotNull] [PathReference] string codeLocation,
			[NotNull] string parameters,
			[NotNull] Dictionary<string, string> localParameters,
			[NotNull] out CancellationTokenSource cancellationToken);
	}
}