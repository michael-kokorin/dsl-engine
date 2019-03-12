namespace Modules.SA.Config
{
	using System;
	using System.IO;

	internal sealed class ScanAgentIdFilePathProvider: IScanAgentIdFilePathProvider
	{
		public string GetIdFilePath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agent.id");
	}
}