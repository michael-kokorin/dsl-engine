namespace Plugins.Git.Vcs.Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using LibGit2Sharp;

	using Infrastructure.Plugins.Contracts;
	using Plugins.Git.Vcs.Extensions;

	using Commit = Infrastructure.Plugins.Contracts.Commit;

	internal sealed class GitRepository
	{
		private readonly string _folderPath;

		public GitRepository([NotNull] string folderPath)
		{
			if (folderPath == null) throw new ArgumentNullException(nameof(folderPath));

			_folderPath = folderPath;
		}

		public void CloneFrom([NotNull] CloneRepository cloneRepository)
		{
			if (cloneRepository == null) throw new ArgumentNullException(nameof(cloneRepository));

			Repository.Clone(
				cloneRepository.Uri,
				_folderPath,
				new CloneOptions
				{
					CredentialsProvider = (url, fromUrl, types) => new UsernamePasswordCredentials
					{
						Password = cloneRepository.Password,
						Username = cloneRepository.Username
					},
					BranchName = cloneRepository.Branch,
					Checkout = true
				});
		}

		public void Commit([NotNull] TakeCommit takeCommit)
		{
			if (takeCommit == null) throw new ArgumentNullException(nameof(takeCommit));

			using (var repo = new Repository(_folderPath))
			{
				repo.Stage(takeCommit.LocalFilePath);

				var author = new Signature(takeCommit.Username, takeCommit.Email, DateTimeOffset.Now);

				repo.Commit(takeCommit.Message, author, author);

				var localBranch = repo.Branches[takeCommit.Branch];

				repo.Network.Push(localBranch,
					new PushOptions
					{
						CredentialsProvider = (url, fromUrl, types) =>
							new UsernamePasswordCredentials
							{
								Password = takeCommit.Password,
								Username = takeCommit.Username
							}
					});
			}
		}

		public BranchInfo CreateBranch([NotNull] CreateBranch createBranch)
		{
			if (createBranch == null) throw new ArgumentNullException(nameof(createBranch));

			using (var repo = new Repository(_folderPath))
			{
				var branch = repo.CreateBranch(createBranch.NewBranchName);

				var localBranch = repo.Branches[createBranch.NewBranchName];

				var remote = repo.Network.Remotes["origin"];

				repo.Branches.Update(
					localBranch,
					b => b.Remote = remote.Name,
					b => b.UpstreamBranch = localBranch.CanonicalName);

				repo.Checkout(localBranch);

				repo.Network.Push(
					localBranch,
					new PushOptions
					{
						CredentialsProvider = (url, fromUrl, types) => new UsernamePasswordCredentials
						{
							Password = createBranch.Password,
							Username = createBranch.Username
						}
					});

				var branchFiendlyName = branch.FriendlyName;

				return new BranchInfo
				{
					Id = branchFiendlyName,
					IsDefault = false,
					Name = branchFiendlyName
				};
			}
		}

		public IEnumerable<Commit> GetCommits([NotNull] IEnumerable<string> branches, [NotNull] GetCommits getCommits)
		{
			if (branches == null) throw new ArgumentNullException(nameof(branches));
			if (getCommits == null) throw new ArgumentNullException(nameof(getCommits));

			using (var repo = new Repository(_folderPath))
			{
				var commitsList = new List<Commit>();

				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				foreach (var branchName in branches)
				{
					 var localBranch = repo.Branches.SingleOrDefault(_ => _.FriendlyName.Equals(branchName));

					if (localBranch == null) continue;

					var commitsQuery = localBranch.Commits.AsQueryable();

					if (getCommits.SinceUtc != null)
						commitsQuery = commitsQuery.Where(_ => _.Committer.When > getCommits.SinceUtc.Value);

					if (getCommits.UntilUtc != null)
						commitsQuery = commitsQuery.Where(_ => _.Committer.When.DateTime < getCommits.UntilUtc.Value);

					var branchCommits = commitsQuery
						.Select(_ =>
						new Commit
						{
							Branch = branchName,
							Committed = _.Committer.When.DateTime,
							Key = _.Sha,
							Title = _.Message
						});

					commitsList.AddRange(branchCommits);
				}

				return commitsList;
			}
		}

		public static IEnumerable<BranchInfo> ListBranches([NotNull] ListBranches listBranches)
		{
			if (listBranches == null) throw new ArgumentNullException(nameof(listBranches));

			var references = Repository
				.ListRemoteReferences(
					listBranches.Url,
					(url, fromUrl, types) => new UsernamePasswordCredentials
					{
						Password = listBranches.Password,
						Username = listBranches.Username
					})
				.ToArray();

			var branches = references
				.Where(elem => elem.IsLocalBranch)
				.Select(
					_ => new BranchInfo
					{
						Name = _.CanonicalName.FromCanonical(),
						Id = _.CanonicalName.FromCanonical(),
						IsDefault = references.Any(
							r =>
								(r.IsLocalBranch == false) &&
								r.CanonicalName.Equals("HEAD") &&
								(r.TargetIdentifier == _.CanonicalName))
					});

			return branches;
		}
	}
}