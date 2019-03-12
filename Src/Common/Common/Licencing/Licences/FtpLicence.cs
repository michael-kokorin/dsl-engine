namespace Common.Licencing.Licences
{
	using System.Collections.Generic;

	internal sealed class FtpLicence : Licence
	{
		public override string Description => "AI SSDL FTP edition";

		public override string Id => LicenceIds.Ftp;

		public FtpLicence() : base(
			new ILicenceComponent[]
			{
				new PluginLicenceComponent(new[] {"Plugins.Ftp.FtpVcsPlugin"}),
				new UserInterfaceLicenceComponent(new Dictionary<string, string>
				{
					{UserCapabilityKey.EnableCommitToVcs, false.ToString()},
					{UserCapabilityKey.EnableCommitToIt, false.ToString()}
				})
			})
		{

		}
	}
}