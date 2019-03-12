namespace Plugins.GitHub.Vcs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;

	using JetBrains.Annotations;

	using Octokit;

	using Infrastructure.Plugins.Contracts;
	using Plugins.GitHub.Vcs.Extensions;

	using Commit = Infrastructure.Plugins.Contracts.Commit;

	// ReSharper disable once MemberCanBeInternal
	public sealed class GitHubVcsPlugin : GitHubPlugin, IVersionControlPlugin
	{
		private const string DefaultBranchName = "master";

		private const string ZipFileExtension = "zip";

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public override string Title => "GitHub";

		public void GetSources(string branchId, string targetPath)
		{
			if (string.IsNullOrEmpty(branchId))
			{
				throw new ArgumentNullException(nameof(branchId));
			}

			if (string.IsNullOrEmpty(targetPath))
			{
				throw new ArgumentNullException(nameof(targetPath));
			}

			var client = GetClient();

			var commitSha = client.Repository.GetBranch(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				branchId)
				.Result.Commit.Sha;

			var sources = client.Repository.Content.GetArchive(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				ArchiveFormat.Zipball,
				commitSha,
				TimeSpan.FromMinutes(60))
				.Result;

			if (!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			SaveFile(branchId, commitSha, targetPath, sources);
		}

		public IEnumerable<BranchInfo> GetBranches()
		{
			var client = GetClient();

			var repoInfo = client.Repository.Get(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName))
				.Result;

			if (repoInfo == null)
			{
				return new BranchInfo[0];
			}

			var branches = client.Repository.GetAllBranches(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName))
				.Result
				.Select(_ => _.ToModel())
				.ToArray();

			var defaultBranch = branches.SingleOrDefault(_ => _.Name == repoInfo.DefaultBranch);

			if (defaultBranch != null)
			{
				defaultBranch.IsDefault = true;
			}

			return branches;
		}

		public BranchInfo CreateBranch(string folderPath, string displayName, string parentBranchId)
		{
			if (string.IsNullOrEmpty(displayName))
			{
				throw new ArgumentNullException(nameof(displayName));
			}

			if ((parentBranchId != null) &&
			    string.IsNullOrEmpty(parentBranchId))
			{
				throw new ArgumentException(nameof(parentBranchId));
			}

			var client = GetClient();

			var parentBranchSha = GetParentBranchSha(parentBranchId, client);

			var newReference = new NewReference(
				displayName.ToBranchName(),
				parentBranchSha);

			client.Git.Reference
				.Create(
					GetSetting(GitHubItSettingKeys.RepositoryOwner),
					GetSetting(GitHubItSettingKeys.RepositoryName),
					newReference)
				.Wait();

			var branch = client.Repository
				.GetBranch(
					GetSetting(GitHubItSettingKeys.RepositoryOwner),
					GetSetting(GitHubItSettingKeys.RepositoryName),
					displayName)
				.Result;

			if (branch == null)
			{
				return null;
			}

			var branchInfo = branch.ToModel();

			var repoInfo = client.Repository.Get(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName))
				.Result;

			branchInfo.IsDefault = repoInfo.DefaultBranch == branchInfo.Name;

			return branchInfo;
		}

		public void Commit(
			string folderPath,
			string branchId,
			string message,
			string fileName,
			byte[] fileBody)
		{
			var client = GetClient();

			var branch = client.Repository.GetBranch(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				branchId)
				.Result;

			var latestCommitSha = branch.Commit.Sha;

			var baseCommit = client.Repository.Commit.Get(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				latestCommitSha)
				.Result;

			var baseTreeSha = baseCommit.Commit.Tree.Sha;

			var newBlob = new NewBlob
			{
				Content = Convert.ToBase64String(fileBody),
				Encoding = EncodingType.Base64
			};

			var blob = client.Git.Blob.Create(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				newBlob)
				.Result;

			var newTree = new NewTree
			{
				BaseTree = baseTreeSha,
				Tree =
				{
					new NewTreeItem
					{
						Path = fileName.ToGitPath(),
						Sha = blob.Sha,
						Type = TreeType.Blob,
						Mode = "100644"
					}
				}
			};

			var tree = client.Git.Tree.Create(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				newTree)
				.Result;

			var newCommit = new NewCommit(
				message,
				tree.Sha,
				new[]
				{
					latestCommitSha
				}
				);

			var commit = client.Git.Commit.Create(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				newCommit)
				.Result;

			client.Git.Reference.Update(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				branchId.ToBranchName(),
				new ReferenceUpdate(commit.Sha))
				.Wait();
		}

		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
		{
			var client = GetClient();

			var branches = client.Repository.GetAllBranches(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName))
				.Result;

			if (!branches.Any())
			{
				return new Commit[0];
			}

			var commits = new List<Commit>();

			foreach (var branch in branches)
			{
				var checkins = client.Repository.Commit.GetAll(
					GetSetting(GitHubItSettingKeys.RepositoryOwner),
					GetSetting(GitHubItSettingKeys.RepositoryName),
					new CommitRequest
					{
						Sha = branch.Name,
						Since = sinceUtc,
						Until = untilUtc
					}).Result;

				commits.AddRange(
					checkins.Select(
						_ => new Commit
						{
							Branch = branch.Name,
							Committed = _.Commit.Committer.Date.DateTime,
							Key = _.Sha,
							Title = _.Commit.Message
						}));
			}

			return commits;
		}

		/// <summary>
		///     Cleans up plugin
		/// </summary>
		public void CleanUp(string folderPath)
		{
			// do nothing
		}

		private static string GetArchiveFileName([NotNull] string branchId, [NotNull] [PathReference] string targetPath)
		{
			var fileName = Path.Combine(targetPath, branchId);

			fileName = Path.ChangeExtension(fileName, ZipFileExtension);

			return fileName;
		}

		private string GetParentBranchSha([CanBeNull] string parentBranchId, [NotNull] IGitHubClient client)
		{
			var parentBranch = client.Repository
				.GetBranch(
					GetSetting(GitHubItSettingKeys.RepositoryOwner),
					GetSetting(GitHubItSettingKeys.RepositoryName),
					parentBranchId ?? DefaultBranchName)
				.Result;

			return parentBranch.Commit.Sha;
		}

		[NotNull]
		private string GetZipFolderName([NotNull] string commitSha, [NotNull] [PathReference] string targetPath)
		{
			var sourceFolderName = ArchiveFolderNameBuilder.Build(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				commitSha);

			var folderName = Path.Combine(targetPath, sourceFolderName);

			return folderName;
		}

		private static void MoveSources([NotNull] [PathReference] string targetPath)
		{
			var folderName = Directory.GetDirectories(targetPath).First();

			folderName.CopyTo(targetPath);

			Directory.Delete(folderName, true);
		}

		private void SaveFile(
			[NotNull] string branchId,
			[NotNull] string commitSha,
			[NotNull] [PathReference] string targetPath,
			[NotNull] byte[] sources)
		{
			var fileName = GetArchiveFileName(branchId, targetPath);

			File.WriteAllBytes(fileName, sources);

			var folderName = GetZipFolderName(commitSha, targetPath);

			if (Directory.Exists(folderName))
			{
				Directory.Delete(folderName, true);
			}

			ZipFile.ExtractToDirectory(fileName, targetPath);

			File.Delete(fileName);

			MoveSources(targetPath);
		}
	}
}