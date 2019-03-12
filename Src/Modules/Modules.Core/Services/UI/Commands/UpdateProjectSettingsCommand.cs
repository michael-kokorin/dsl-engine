namespace Modules.Core.Services.UI.Commands
{
	using Common.Command;

	internal sealed class UpdateProjectSettingsCommand : ICommand
	{
		public string Alias { get; set; }

		public bool CommitToIt { get; set; }

		public bool CommitToVcs { get; set; }

		public string DefaultBranchName { get; set; }

		public string DisplayName { get; set; }

		public string Description { get; set; }

		public long ProjectId { get; set; }

		public bool VcsSyncEnabled { get; set; }

		public bool EnablePoll { get; set; }

		public int? PollTimeout { get; set; }
	}
}