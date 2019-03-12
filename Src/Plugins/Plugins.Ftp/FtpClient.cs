namespace Plugins.Ftp
{
	using System;
	using System.IO;
	using System.Net;

	internal sealed class FtpClient : IDisposable
	{
		private readonly string _uri;

		private readonly string _userName;

		private readonly string _userPassword;

		private readonly bool _sslEnabled;

		private FtpWebRequest _ftpWebRequest;

		private TextReader _streamReader;

		private FtpWebResponse _ftpWebResponse;

		private bool _isDisposed;

		public FtpClient(string uri, string userName, string userPassword, bool sslEnabled)
		{
			_sslEnabled = sslEnabled;

			_uri = uri;

			_userName = userName;

			_userPassword = userPassword;
		}

		~FtpClient()
		{
			Dispose(false);
		}

		public FtpWebRequest CreateRequest(string method)
		{
			_ftpWebRequest = (FtpWebRequest)WebRequest.Create(_uri);
			_ftpWebRequest.Method = method;

			_ftpWebRequest.Credentials = new NetworkCredential(
				_userName,
				_userPassword);

			_ftpWebRequest.UseBinary = true;
			_ftpWebRequest.EnableSsl = _sslEnabled;

			return _ftpWebRequest;
		}

		public TextReader ExecuteMethod(string method)
		{
			var request = CreateRequest(method);

			_ftpWebResponse = (FtpWebResponse) request.GetResponse();

			var responseStream = _ftpWebResponse.GetResponseStream();

			if (responseStream == null)
				return null;

			_streamReader = new StreamReader(responseStream);

			return _streamReader;
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				if (_streamReader != null)
				{
					_streamReader.Close();

					_streamReader.Dispose();
				}

				if (_ftpWebResponse != null)
				{
					_ftpWebResponse.Close();

					_ftpWebResponse.Dispose();
				}
			}

			_isDisposed = true;
		}
	}
}