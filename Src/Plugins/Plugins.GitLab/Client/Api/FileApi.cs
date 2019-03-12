namespace Plugins.GitLab.Client.Api
{
	using System;
	using System.Threading.Tasks;

	using Plugins.GitLab.Client.Entity;

	public sealed class FileApi : BaseApi
	{
		public FileApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<File>> Update(long projectId, UpdateFile updateFile)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/files", HttpMethod.Put);

			var content = Convert.ToBase64String(updateFile.Content);

			request.AddUrlSegment("projectId", projectId);
			request.AddParameter("branch_name", updateFile.BranchName);
			request.AddParameter("file_path", updateFile.FilePath);
			request.AddParameter("commit_message", updateFile.CommitMessage);
			request.AddParameter("content", content);
			request.AddParameter("encoding", "base64");

			return await request.Execute<File>();
		}
	}
}