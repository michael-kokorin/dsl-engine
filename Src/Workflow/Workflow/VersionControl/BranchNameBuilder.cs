namespace Workflow.VersionControl
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Workflow.Properties;

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
	internal sealed class BranchNameBuilder: IBranchNameBuilder
	{
		public string Build(BranchNameBuilderInfo info)
			=> Resources.BranchNameFormat.FormatWith(info.Type.ToLower().Replace(' ', '_'));

		public bool IsOurBranch(string name)
		{
			var lastName = name.Substring(name.LastIndexOf('/') + 1);

			return string.Join(" ", lastName.ToLower().Split('_').Take(2)) ==
					string.Join(" ", Resources.BranchNameFormat.ToLower().Split('_').Take(2));
		}

		public BranchNameBuilderInfo GetInfo(string name) =>
			new BranchNameBuilderInfo
			{
				Type = string.Join(" ", name.Split('_').Skip(2))
			};
	}
}