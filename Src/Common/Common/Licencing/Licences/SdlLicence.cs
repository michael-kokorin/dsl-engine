namespace Common.Licencing.Licences
{
	using System.Collections.Generic;

	internal sealed class SdlLicence : Licence
	{
		public override string Description => "AI SSDL Enterprise edition. No limits, no horizonts";

		public override string Id => LicenceIds.Sdl;

		public SdlLicence() : base(
			new ILicenceComponent[]
			{
				new PluginLicenceComponent(),
				new UserInterfaceLicenceComponent(new Dictionary<string, string>
				{
					{UserCapabilityKey.EnableCommitToVcs, true.ToString()},
					{UserCapabilityKey.EnableCommitToIt, true.ToString()}
				})
			})
		{

		}
	}
}