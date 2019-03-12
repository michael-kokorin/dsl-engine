namespace Repository.Context
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Common.Enums;
	using Common.Extensions;
	using Repository.Properties;

	/// <summary>
	///   Represents task entity.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	[ProjectProperty("ProjectId")]
	public partial class Tasks: IEntity
	{
		/// <summary>
		///   Gets a value indicating whether this instance is cancelled.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this instance is cancelled; otherwise, <see langword="false"/>.
		/// </value>
		[NotMapped]
		public bool IsCancelled => (TaskStatus == TaskStatus.Done) && (TaskResolutionStatus == TaskResolutionStatus.Cancelled)
			;

		/// <summary>
		///   Gets or sets the SDL policy status.
		/// </summary>
		/// <value>
		///   The SDL policy status.
		/// </value>
		/// <remarks>DO NOT USE IN QUERIES. USE <see cref="SdlStatus"/> INSTEAD.</remarks>
		public SdlPolicyStatus SdlPolicyStatus
		{
			get { return (SdlPolicyStatus)SdlStatus; }
			set { SdlStatus = (int)value; }
		}

		/// <summary>
		///   Gets the task resolution status.
		/// </summary>
		/// <value>
		///   The task resolution status.
		/// </value>
		/// <remarks>DO NOT USE IN QUERIES. USE <see cref="Resolution"/> INSTEAD.</remarks>
		[NotMapped]
		public TaskResolutionStatus TaskResolutionStatus
		{
			get { return (TaskResolutionStatus)Resolution; }
			set { Resolution = (int)value; }
		}

		/// <summary>
		///   Gets the task status.
		/// </summary>
		/// <value>
		///   The task status.
		/// </value>
		/// <remarks>DO NOT USE IN QUERIES. USE <see cref="Status"/> INSTEAD.</remarks>
		[NotMapped]
		public TaskStatus TaskStatus
		{
			get { return (TaskStatus)Status; }
			set { Status = (int)value; }
		}

		/// <summary>
		///   Cancels the task.
		/// </summary>
		public void Cancel(string message = null)
		{
			if (TaskStatus == TaskStatus.Done)
				throw new InvalidOperationException(); // TODO: message

			TaskStatus = TaskStatus.Done;
			TaskResolutionStatus = TaskResolutionStatus.Cancelled;
			ResolutionMessage = message;
		}

		/// <summary>
		///   Completes the task successfully.
		/// </summary>
		public void FinishPostProcessing(DateTime finishDateUtc)
		{
			if(TaskStatus != TaskStatus.PostProcessing)
				throw new InvalidOperationException(Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.PostProcessing));

			if (finishDateUtc < Created)
				throw new ArgumentException(nameof(finishDateUtc));

			TaskStatus = TaskStatus.Done;
			Finished = finishDateUtc;
			TaskResolutionStatus = TaskResolutionStatus.Successful;
			SdlPolicyStatus = SdlPolicyStatus.Unknown;
		}

		/// <summary>
		///   Fails the task with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Fail(string message = null)
		{
			if (TaskStatus == TaskStatus.Done)
				throw new InvalidOperationException();

			TaskStatus = TaskStatus.Done;
			TaskResolutionStatus = TaskResolutionStatus.Error;
			ResolutionMessage = message;
		}

		/// <summary>
		///   Finishes the preprocessing.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///   State is different from
		///   <see cref="Common.Enums.TaskStatus.PreProcessing"/>
		/// </exception>
		public void FinishPreprocessing(string folderPath, long folderSize)
		{
			if(TaskStatus != TaskStatus.PreProcessing)
				throw new InvalidOperationException(Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.PreProcessing));

			TaskStatus = TaskStatus.ReadyToScan;
			TaskResolutionStatus = TaskResolutionStatus.InProgress;
			FolderPath = folderPath;
			FolderSize = folderSize;
		}

		/// <summary>
		///   Finishes the scanning.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///   State is different from <see cref="Common.Enums.TaskStatus.Scanning"/>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///   State is different from <see cref="Common.Enums.TaskStatus.PostProcessing"/>
		/// </exception>
		public void FinishScanning()
		{
			if(TaskStatus != TaskStatus.Scanning)
				throw new InvalidOperationException(Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.Scanning));

			TaskStatus = TaskStatus.ReadyToPostProcessing;
			TaskResolutionStatus = TaskResolutionStatus.InProgress;
		}

		/// <summary>
		///   Starts the post processing.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///   State is different from
		///   <see cref="Common.Enums.TaskStatus.ReadyToPostProcessing"/>
		/// </exception>
		public void StartPostProcessing()
		{
			if(TaskStatus != TaskStatus.ReadyToPostProcessing)
			{
				throw new InvalidOperationException(
					Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.ReadyToPostProcessing));
			}

			TaskStatus = TaskStatus.PostProcessing;
			TaskResolutionStatus = TaskResolutionStatus.InProgress;
		}

		/// <summary>
		///   Starts the preprocessing.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///   State is different from <see cref="Common.Enums.TaskStatus.New"/>
		/// </exception>
		public void StartPreprocessing()
		{
			if(TaskStatus != TaskStatus.New)
				throw new InvalidOperationException(Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.New));

			TaskStatus = TaskStatus.PreProcessing;
			TaskResolutionStatus = TaskResolutionStatus.InProgress;
		}

		/// <summary>
		///   Starts the scanning.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///   State is different from <see cref="Common.Enums.TaskStatus.ReadyToScan"/>
		/// </exception>
		public void StartScanning(long agentId)
		{
			if(TaskStatus != TaskStatus.ReadyToScan)
				throw new InvalidOperationException(Resources.WrongTaskStatus.FormatWith(TaskStatus, TaskStatus.ReadyToScan));

			TaskStatus = TaskStatus.Scanning;
			TaskResolutionStatus = TaskResolutionStatus.InProgress;
			AgentId = agentId;
		}

		public void ViolatePolicy(string resolutionMessage)
		{
			SdlPolicyStatus = SdlPolicyStatus.Failed;
			Projects.SdlPolicyStatus = (int)SdlPolicyStatus.Failed;
			ResolutionMessage = resolutionMessage;
		}

		public void SuccessPolicy()
		{
			SdlPolicyStatus = SdlPolicyStatus.Success;
			Projects.SdlPolicyStatus = (int)SdlPolicyStatus.Success;
		}

		public void ErrorPolicy(string errorMessage)
		{
			SdlPolicyStatus = SdlPolicyStatus.Error;
			Projects.SdlPolicyStatus = (int)SdlPolicyStatus.Error;
			ResolutionMessage = errorMessage;
		}
	}
}