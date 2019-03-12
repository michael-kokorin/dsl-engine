namespace Infrastructure.AD
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;
	using System.Security.Permissions;
	using System.Security.Principal;

	internal sealed class WrapperImpersonationContext
	{
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(
			string lpszUsername,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);

		private const int LOGON32_PROVIDER_DEFAULT = 0;

		private const int LOGON32_LOGON_INTERACTIVE = 2;

		private string m_Domain;

		private string m_Password;

		private string m_Username;

		private IntPtr m_Token;

		private WindowsImpersonationContext m_Context;

		private bool IsInContext => m_Context != null;

		public WrapperImpersonationContext(string domain, string username, string password)
		{
			m_Domain = domain;
			m_Username = username;
			m_Password = password;
		}

		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public void Enter()
		{
			if (IsInContext) return;

			m_Token = new IntPtr(0);

			m_Token = IntPtr.Zero;

			var logonSuccessfull = LogonUser(
				m_Username,
				m_Domain,
				m_Password,
				LOGON32_LOGON_INTERACTIVE,
				LOGON32_PROVIDER_DEFAULT,
				ref m_Token);

			if (logonSuccessfull == false)
			{
				var error = Marshal.GetLastWin32Error();

				throw new Win32Exception(error);
			}
			var identity = new WindowsIdentity(m_Token);

			m_Context = identity.Impersonate();
		}

		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public void Leave()
		{
			if (IsInContext == false) return;

			m_Context.Undo();

			if (m_Token != IntPtr.Zero) CloseHandle(m_Token);

			m_Context = null;
		}
	}
}