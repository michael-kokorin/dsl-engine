namespace Plugins.Git.Vcs.Client
{
	using System;

	internal sealed class GetCommits
	{
		public readonly DateTime? SinceUtc;

		public readonly DateTime? UntilUtc;

		public GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
		{
			SinceUtc = sinceUtc;
			UntilUtc = untilUtc;
		}
	}
}