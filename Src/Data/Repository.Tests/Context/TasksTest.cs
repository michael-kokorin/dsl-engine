namespace Repository.Tests.Context
{
	using System;

	using FluentAssertions;

	using NUnit.Framework;

	using Common.Enums;
	using Repository.Context;

	[TestFixture]
	public sealed class TasksTest
	{
		[Test]
		public void CancelTest()
		{
			const string message = "message";
			var task = new Tasks();

			task.Cancel(message);

			task.TaskStatus.Should().Be(TaskStatus.Done);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.Cancelled);
			task.ResolutionMessage.Should().Be(message);
		}

		[Test]
		public void FailTaskTest()
		{
			const string message = "message";
			var task = new Tasks();

			task.Fail(message);

			task.TaskStatus.Should().Be(TaskStatus.Done);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.Error);
			task.ResolutionMessage.Should().Be(message);
		}

		[Test]
		[TestCase(TaskStatus.New)]
		[TestCase(TaskStatus.ReadyToScan)]
		[TestCase(TaskStatus.Scanning)]
		[TestCase(TaskStatus.ReadyToPostProcessing)]
		[TestCase(TaskStatus.PostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void FinishPreprocessingTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			const string path = "path";
			const long size = 1L;

			Assert.Throws<InvalidOperationException>(() => task.FinishPreprocessing(path, size));
		}

		[Test]
		public void FinishPreprocessingTestWithPreprocessing()
		{
			var task = new Tasks
			{
				Status = (int)TaskStatus.PreProcessing
			};

			const string path = "path";
			const long size = 1L;

			task.FinishPreprocessing(path, size);

			task.TaskStatus.Should().Be(TaskStatus.ReadyToScan);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.InProgress);
			task.FolderPath.Should().Be(path);
			task.FolderSize.Should().Be(size);
		}

		[Test]
		[TestCase(TaskStatus.New)]
		[TestCase(TaskStatus.PreProcessing)]
		[TestCase(TaskStatus.ReadyToScan)]
		[TestCase(TaskStatus.ReadyToPostProcessing)]
		[TestCase(TaskStatus.PostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void FinishScanningTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			Assert.Throws<InvalidOperationException>(() => task.FinishScanning());
		}

		[Test]
		public void FinishScanningTestWithScanning()
		{
			var task = new Tasks
			{
				Status = (int)TaskStatus.Scanning
			};

			task.FinishScanning();

			task.TaskStatus.Should().Be(TaskStatus.ReadyToPostProcessing);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.InProgress);
		}

		[Test]
		[TestCase(TaskStatus.New)]
		[TestCase(TaskStatus.PreProcessing)]
		[TestCase(TaskStatus.ReadyToScan)]
		[TestCase(TaskStatus.Scanning)]
		[TestCase(TaskStatus.ReadyToPostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void FinishTaskTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			Assert.Throws<InvalidOperationException>(() => task.FinishPostProcessing(DateTime.UtcNow));
		}

		[Test]
		public void FinishTaskTestWithPostProcessing()
		{
			var finishDate = DateTime.UtcNow;

			var task = new Tasks
			{
				Status = (int)TaskStatus.PostProcessing
			};

			task.FinishPostProcessing(finishDate);

			task.TaskStatus.Should().Be(TaskStatus.Done);
			task.Finished.ShouldBeEquivalentTo(finishDate);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.Successful);
		}

		[Test]
		[TestCase(TaskStatus.New, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.PreProcessing, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.ReadyToScan, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.Scanning, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.ReadyToPostProcessing, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.PostProcessing, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.Done, TaskResolutionStatus.New, false)]
		[TestCase(TaskStatus.New, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.PreProcessing, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.ReadyToScan, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.Scanning, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.ReadyToPostProcessing, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.PostProcessing, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.Done, TaskResolutionStatus.InProgress, false)]
		[TestCase(TaskStatus.New, TaskResolutionStatus.Cancelled, false)]
		[TestCase(TaskStatus.PreProcessing, TaskResolutionStatus.Cancelled, false)]
		[TestCase(TaskStatus.ReadyToScan, TaskResolutionStatus.Cancelled, false)]
		[TestCase(TaskStatus.Scanning, TaskResolutionStatus.Cancelled, false)]
		[TestCase(TaskStatus.ReadyToPostProcessing, TaskResolutionStatus.Cancelled, false)]
		[TestCase(TaskStatus.PostProcessing, TaskResolutionStatus.Cancelled, false)]

		// only this test should be true
		[TestCase(TaskStatus.Done, TaskResolutionStatus.Cancelled, true)]
		[TestCase(TaskStatus.New, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.PreProcessing, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.ReadyToScan, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.Scanning, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.ReadyToPostProcessing, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.PostProcessing, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.Done, TaskResolutionStatus.Successful, false)]
		[TestCase(TaskStatus.New, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.PreProcessing, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.ReadyToScan, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.Scanning, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.ReadyToPostProcessing, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.PostProcessing, TaskResolutionStatus.Error, false)]
		[TestCase(TaskStatus.Done, TaskResolutionStatus.Error, false)]
		public void IsCancelled(TaskStatus status, TaskResolutionStatus resolutionStatus, bool result)
		{
			var task = new Tasks
			{
				Status = (int)status,
				Resolution = (int)resolutionStatus
			};

			task.IsCancelled.Should().Be(result);
		}

		[Test]
		[TestCase(TaskStatus.New)]
		[TestCase(TaskStatus.PreProcessing)]
		[TestCase(TaskStatus.ReadyToScan)]
		[TestCase(TaskStatus.Scanning)]
		[TestCase(TaskStatus.PostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void StartPostprocessingTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			Assert.Throws<InvalidOperationException>(() => task.StartPostProcessing());
		}

		[Test]
		public void StartPostProcessTestWithReadyPostProcessing()
		{
			var task = new Tasks
			{
				Status = (int)TaskStatus.ReadyToPostProcessing
			};

			task.StartPostProcessing();

			task.TaskStatus.Should().Be(TaskStatus.PostProcessing);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.InProgress);
		}

		[Test]
		[TestCase(TaskStatus.PreProcessing)]
		[TestCase(TaskStatus.ReadyToScan)]
		[TestCase(TaskStatus.Scanning)]
		[TestCase(TaskStatus.ReadyToPostProcessing)]
		[TestCase(TaskStatus.PostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void StartPreprocessingTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			Assert.Throws<InvalidOperationException>(() => task.StartPreprocessing());
		}

		[Test]
		public void StartPreProcessTestWithNew()
		{
			var task = new Tasks
			{
				Status = (int)TaskStatus.New
			};

			task.StartPreprocessing();

			task.TaskStatus.Should().Be(TaskStatus.PreProcessing);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.InProgress);
		}

		[Test]
		[TestCase(TaskStatus.New)]
		[TestCase(TaskStatus.PreProcessing)]
		[TestCase(TaskStatus.Scanning)]
		[TestCase(TaskStatus.ReadyToPostProcessing)]
		[TestCase(TaskStatus.PostProcessing)]
		[TestCase(TaskStatus.Done)]
		public void StartScanningTestWithOtherStatuses(TaskStatus status)
		{
			var task = new Tasks
			{
				Status = (int)status
			};

			Assert.Throws<InvalidOperationException>(() => task.StartScanning(1));
		}

		[Test]
		public void StartScanningTestWithReadyScan()
		{
			const long agentId = 234;

			var task = new Tasks
			{
				Status = (int)TaskStatus.ReadyToScan
			};

			task.StartScanning(agentId);

			task.TaskStatus.Should().Be(TaskStatus.Scanning);
			task.TaskResolutionStatus.Should().Be(TaskResolutionStatus.InProgress);
			task.AgentId.ShouldBeEquivalentTo(agentId);
		}
	}
}