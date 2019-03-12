namespace PT.Sdl.Infrastructure.Plugins.Agent.Contracts
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ScanStatus")]
	public enum ScanStatus
	{
		[EnumMember]
		Success = 0,

		[EnumMember]
		Failed = 1,

		[EnumMember]
		Stopped = 2
	}
}