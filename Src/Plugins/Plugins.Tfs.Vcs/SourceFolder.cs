namespace Plugins.Tfs.Vcs
{
	using System;
	using System.IO;

	using JetBrains.Annotations;

	internal sealed class SourceFolder
	{
		private readonly string _rootPath;

		private readonly string _branchId;

		private const string TempFolderName = "$br";

		private string _branchFolderName;

		public string BranchFolderName
		{
			get
			{
				if (string.IsNullOrEmpty(_branchFolderName))
				{
					_branchFolderName = Path.Combine(_rootPath, TempFolderName, _branchId);
				}

				return _branchFolderName;
			}
		}

		private SourceFolder(string rootPath, string branchId)
		{
			_rootPath = rootPath;

			_branchId = branchId;

			if (!Directory.Exists(BranchFolderName))
				Directory.CreateDirectory(BranchFolderName);
		}

		public static SourceFolder Create(string rootPath, string branchId) => new SourceFolder(rootPath, branchId);

		public string SaveFile([NotNull] string fileName, byte[] fileContent)
		{
			if (fileName == null) throw new ArgumentNullException(nameof(fileName));

			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(nameof(fileName));

			var filePath = Path.Combine(BranchFolderName, fileName);

			if (File.Exists(filePath))
				File.Delete(filePath);

			File.WriteAllBytes(filePath, fileContent);

			return filePath;
		}

		public bool Exists([NotNull] string fileName)
		{
			if (fileName == null) throw new ArgumentNullException(nameof(fileName));

			var filePath = Path.Combine(BranchFolderName, fileName);

			return File.Exists(filePath);
		}
	}
}