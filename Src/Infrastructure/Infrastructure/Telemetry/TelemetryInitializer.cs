namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;

	using Common.Security;
	using Repository;

	[UsedImplicitly]
	internal sealed class TelemetryInitializer : ITelemetryInitializer
	{
		private readonly IUserPrincipal _userPrincipal;

		public TelemetryInitializer([NotNull] IUserPrincipal userPrincipal)
		{
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_userPrincipal = userPrincipal;
		}

		public void Initialize<T>([NotNull] T telemetry) where T : class, ITelemetry
		{
			if (telemetry == null) throw new ArgumentNullException(nameof(telemetry));

			telemetry.DateTimeLocal = DateTime.Now;
			telemetry.DateTimeUtc = DateTime.UtcNow;

			telemetry.UserLogin = _userPrincipal.Info.Login;
			telemetry.UserSid = _userPrincipal.Info.Sid;
		}
	}
}