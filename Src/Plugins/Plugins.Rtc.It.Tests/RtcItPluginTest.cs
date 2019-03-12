namespace Plugins.Rtc.It.Tests
{
	using System.Collections.Generic;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;
	using Plugins.Rtc.It;

	[TestFixture]
	[Ignore("Functional tests")]
	public sealed class RtcItPluginTest
	{
		private IIssueTrackerPlugin _target;

		[SetUp]
		public void SetUp()
		{
			_target = new RtcItPlugin();

			var localSettings = new Dictionary<string, string>
			{
				{
					RtcSettings.Host.ToString(),
					"https://10.5.208.17:9443" //"https://jazz.net"
				},
				{
					RtcSettings.Ccm.ToString(),
					"ccm" //"sandbox01-ccm"
				},
				{
					RtcSettings.Project.ToString(),
					"SDL"
				},
				{
					RtcSettings.Username.ToString(),
					"msharonov"
				},
				{
					RtcSettings.Password.ToString(),
					"P@ssw0rd"
				}
			};

			//var cloudSettings = new Dictionary<string, string>
			//{
			//	{
			//		RtcSettings.Host.ToString(),
			//		"https://jazz.net"
			//	},
			//	{
			//		RtcSettings.Ccm.ToString(),
			//		"sandbox01-ccm"
			//	},
			//	{
			//		RtcSettings.Project.ToString(),
			//		"MsTest"
			//	},
			//	{
			//		RtcSettings.Username.ToString(),
			//		"msharonov"
			//	},
			//	{
			//		RtcSettings.Password.ToString(),
			//		"P@ssw0rd"
			//	}
			//};

			_target.LoadSettingValues(localSettings);
		}

		[Test]
		public void ShouldTakeIssueFromRtc()
		{
			var issue = _target.GetIssue("1");

			issue.Should().NotBeNull();
		}

		[Test]
		public void ShouldTakeIssuesFromRtc()
		{
			var issues = _target.GetIssues();

			issues.Should().NotBeNull();
		}

		[Test]
		public void ShouldCreateIssueInRtc()
		{
			var issue = _target.CreateIssue(null,
				new CreateIssueRequest
				{
					Description = "Test desc",
					Title = "Test title"
				});

			issue.Should().NotBeNull();
		}

		[Test]
		public void ShouldUpdateIssueInRtc()
		{
			var issue = _target.UpdateIssue(new UpdateIssueRequest
			{
				Description = "Desc 2",
				Id = "728",
				Status = IssueStatus.Open,
				Title = "Title 22"
			});

			issue.Should().NotBeNull();
		}
	}
}