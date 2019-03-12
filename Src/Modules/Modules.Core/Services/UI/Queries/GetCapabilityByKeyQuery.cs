namespace Modules.Core.Services.UI.Queries
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;

	internal sealed class GetCapabilityByKeyQuery : IDataQuery
	{
		public readonly string CapabilityKey;

		public GetCapabilityByKeyQuery([NotNull] string capabilityKey)
		{
			if (capabilityKey == null) throw new ArgumentNullException(nameof(capabilityKey));

			CapabilityKey = capabilityKey;
		}
	}
}