namespace Plugins.Ftp
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Web;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;

	public sealed class FtpVcsPlugin: IVersionControlPlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the dictionary of plugin instance settings
		/// </summary>
		/// <returns>Settings dictionary</returns>
		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				Code = "ftp_vcs",
				DisplayName = "FTP VCS",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
									{
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											SettingType = SettingType.Text,
											DisplayName = "User Name",
											Code = FtpSettings.UserName.ToString(),
											DefaultValue = null,
											IsAuthentication = true,
											SettingOwner = SettingOwner.User
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											SettingType = SettingType.Password,
											DisplayName = "User Password",
											Code = FtpSettings.UserPassword.ToString(),
											DefaultValue = null,
											IsAuthentication = true,
											SettingOwner = SettingOwner.User
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											SettingType = SettingType.Text,
											DisplayName = "Host (port)",
											Code = FtpSettings.HostUri.ToString(),
											DefaultValue = null,
											IsAuthentication = true,
											SettingOwner = SettingOwner.Project
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											SettingType = SettingType.Boolean,
											DisplayName = "SSL Enabled",
											Code = FtpSettings.SslEnabled.ToString(),
											DefaultValue = null,
											IsAuthentication = true,
											SettingOwner = SettingOwner.Project
										}
									}
			};

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public string Title => "FTP VCS";

		/// <summary>
		///     Initialize plugin by settings
		/// </summary>
		/// <param name="values">The setting values</param>
		public void LoadSettingValues(IDictionary<string, string> values) => _settingValues = values;

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser() => new User
										{
											DisplayName = GetSetting(FtpSettings.UserName),
											InfoUrl = null,
											Login = GetSetting(FtpSettings.UserName)
										};

		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		public void GetSources(string branchId, string targetPath) => DownloadFolder(branchId, targetPath);

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		public IEnumerable<BranchInfo> GetBranches()
		{
			var ftpFileInfo = GetContent(null).ToArray();

			var result = new List<BranchInfo>();

			result.AddRange(
				ftpFileInfo
					.Where(_ => _.IsDirectory)
					.Select(
						_ => new BranchInfo
							{
								Id = _.FileName,
								IsDefault = false,
								Name = _.FileName
							}));

			const string parentFolder = "/";

			result.Add(
				new BranchInfo
				{
					Id = parentFolder,
					IsDefault = false,
					Name = parentFolder
				});

			return result.OrderBy(_ => _.Id);
		}

		/// <summary>
		///     Creates the branch.
		/// </summary>
		/// <param name="rootFolderPath">Root folder path</param>
		/// <param name="branchDisplayName">The branch display name.</param>
		/// <param name="parentBranchId">The parent branch identifier.</param>
		/// <returns>Information about created branch</returns>
		public BranchInfo CreateBranch(string rootFolderPath, string branchDisplayName, string parentBranchId = null)
		{
			if(rootFolderPath == null)
			{
				throw new ArgumentNullException(nameof(rootFolderPath));
			}
			if(branchDisplayName == null)
			{
				throw new ArgumentNullException(nameof(branchDisplayName));
			}

			var relativePath = branchDisplayName;

			if(!string.IsNullOrEmpty(parentBranchId))
			{
				relativePath = $"{parentBranchId}/{branchDisplayName}";
			}

			UploadFolder(rootFolderPath, relativePath);

			return new BranchInfo
					{
						Id = relativePath,
						IsDefault = false,
						Name = relativePath
					};
		}

		/// <summary>
		///     Commits the file.
		/// </summary>
		/// <param name="folderPath">Root folder path</param>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="message">The commit message.</param>
		/// <param name="fileName">Local name of the file.</param>
		/// <param name="fileBody">The file body.</param>
		public void Commit(string folderPath, string branchId, string message, string fileName, byte[] fileBody)
		{
			var fileRelativePath = $"{branchId}/{fileName}";

			if(CheckFileExists(fileRelativePath))
			{
				DeleteFile(fileRelativePath);
			}

			UploadFile(fileBody, fileRelativePath);
		}

		/// <summary>
		///     Gets checkins history from VCS sinse the last checkin
		/// </summary>
		/// <param name="sinceUtc">The first checkin date</param>
		/// <param name="untilUtc">The last chekin date</param>
		/// <returns>List of checkins sinse the last chekin date</returns>
		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
			=> Enumerable.Empty<Commit>();

		/// <summary>
		///     Cleans up plugin
		/// </summary>
		public void CleanUp(string folderPath)
		{
			// do nothing on ftp
		}

		private bool CheckDirectoryExists(string directoryRelativepath)
		{
			try
			{
				GetContent(directoryRelativepath);

				return true;
			}
			catch(WebException)
			{
				return false;
			}
		}

		private bool CheckFileExists(string fileRelativePath)
		{
			try
			{
				using(var ftpClient = GetFtpClient(fileRelativePath))
				{
					ftpClient.ExecuteMethod(WebRequestMethods.Ftp.GetFileSize);
				}

				return true;
			}
			catch(WebException)
			{
				return false;
			}
		}

		private void CreateDirectory(string directoryName)
		{
			using(var ftpClient = GetFtpClient(directoryName))
			{
				ftpClient.ExecuteMethod(WebRequestMethods.Ftp.MakeDirectory);
			}
		}

		private void DeleteFile(string fileRelativePath)
		{
			using(var ftpClient = GetFtpClient(fileRelativePath))
			{
				ftpClient.ExecuteMethod(WebRequestMethods.Ftp.DeleteFile);
			}
		}

		private void DownloadFile(string branchId, string fileName, string targetFolder)
		{
			var fileRelativePath = $"{branchId}/{fileName}";

			using(var ftpClient = GetFtpClient(fileRelativePath))
			{
				var request = ftpClient.CreateRequest(WebRequestMethods.Ftp.DownloadFile);

				using(var response = (FtpWebResponse)request.GetResponse())
				{
					using(var responseStream = response.GetResponseStream())
					{
						if(responseStream != null)
						{
							var localFileName = Path.Combine(targetFolder, fileName);

							using(var writeStream = new FileStream(localFileName, FileMode.Create))
							{
								const int bufferLength = 2048;

								var buffer = new byte[bufferLength];

								var bytesRead = responseStream.Read(buffer, 0, bufferLength);

								while(bytesRead > 0)
								{
									writeStream.Write(buffer, 0, bytesRead);

									bytesRead = responseStream.Read(buffer, 0, bufferLength);
								}

								writeStream.Close();
							}
						}
					}

					response.Close();
				}
			}
		}

		private void DownloadFolder(string brandhId, string targetPath)
		{
			if(!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			var content = GetContent(brandhId).ToArray();

			var files = content.Where(_ => !_.IsDirectory);

			foreach(var file in files)
			{
				var fileLocalPath = Path.Combine(targetPath, file.FileName);

				if(File.Exists(fileLocalPath))
				{
					File.Delete(fileLocalPath);
				}

				DownloadFile(brandhId, file.FileName, targetPath);
			}

			var directories = content.Where(_ => _.IsDirectory);

			foreach(var directory in directories)
			{
				var branchName = $"{brandhId}/{directory.FileName}";

				var localDirectoryPath = Path.Combine(targetPath, directory.FileName);

				DownloadFolder(branchName, localDirectoryPath);
			}
		}

		private IEnumerable<FtpFileInfo> GetContent(string relativePath)
		{
			string response;

			using(var ftpClient = GetFtpClient(relativePath))
			{
				var reader = ftpClient.ExecuteMethod(WebRequestMethods.Ftp.ListDirectoryDetails);

				response = reader.ReadToEnd();
			}

			if(string.IsNullOrEmpty(response))
			{
				return Enumerable.Empty<FtpFileInfo>();
			}

			var ftpFileInfo = FtpFileInfo.Parse(response);

			return ftpFileInfo;
		}

		private FtpClient GetFtpClient(string relativePath = null)
		{
			var hostAddress = GetSetting(FtpSettings.HostUri);

			var hostUri = new Uri($"ftp://{hostAddress}", UriKind.Absolute);

			if(!string.IsNullOrEmpty(relativePath))
			{
				hostUri = new Uri(hostUri, (HttpUtility.UrlEncode(relativePath) ?? string.Empty).Replace("+", " "));
			}

			var sslEnabled = bool.Parse(GetSetting(FtpSettings.SslEnabled));

			return new FtpClient(
				hostUri.AbsoluteUri,
				GetSetting(FtpSettings.UserName),
				GetSetting(FtpSettings.UserPassword),
				sslEnabled);
		}

		private string GetSetting(FtpSettings setting) => _settingValues[setting.ToString()];

		private void UploadFile(byte[] fileBody, string fileRelativePath)
		{
			using(var ftpClient = GetFtpClient(fileRelativePath))
			{
				var request = ftpClient.CreateRequest(WebRequestMethods.Ftp.UploadFile);

				request.ContentLength = fileBody.Length;

				var requestStream = request.GetRequestStream();
				requestStream.Write(fileBody, 0, fileBody.Length);
				requestStream.Close();

				request.GetResponse();
			}
		}

		private void UploadFolder([NotNull] string rootFolderPath, string uploadPath)
		{
			if(!CheckDirectoryExists(uploadPath))
			{
				CreateDirectory(uploadPath);
			}

			var files = Directory.GetFiles(rootFolderPath, "*.*");

			foreach(var file in files)
			{
				var fileContent = File.ReadAllBytes(file);

				var fileName = Path.GetFileName(file);

				var relativeFilePath = $"{uploadPath}/{fileName}";

				if(CheckFileExists(relativeFilePath))
				{
					DeleteFile(relativeFilePath);
				}

				Commit(rootFolderPath, uploadPath, null, fileName, fileContent);
			}

			var subDirs = Directory.GetDirectories(rootFolderPath);

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach(var subDir in subDirs)
			{
				var directoryName = Path.GetFileName(subDir);

				if(string.IsNullOrEmpty(directoryName))
				{
					continue;
				}

				UploadFolder(Path.Combine(rootFolderPath, directoryName), uploadPath + "/" + directoryName);
			}
		}
	}
}