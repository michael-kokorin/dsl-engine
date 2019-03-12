namespace Common.SystemComponents
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class AssemblySystemVersionProvider : ISystemVersionProvider
	{
		public string GetSystemVersion() => GetType().Assembly.GetName().Version.ToString();
	}
}