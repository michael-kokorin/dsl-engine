namespace Plugins.Rtc.It.Client.Api
{
	/// <summary>
	/// Create IBM CCM work item requiest
	/// </summary>
	public sealed class CreateWorkItem
	{
		/// <summary>
		/// Gets or sets work item description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the filed against resource.
		/// 
		/// For ex. https://jazz.net/sandbox01-ccm/resource/itemOid/com.ibm.team.workitem.Category/_Abs2kF3hEeaDUp2Dd41-Rw
		/// </summary>
		/// <value>
		/// The filed against resource.
		/// </value>
		public string FiledAgainstResource { get; set; }

		/// <summary>
		/// Gets or sets work item title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the type resource.
		/// 
		/// For ex. https://jazz.net/sandbox01-ccm/oslc/types/_AJByMF3hEeaDUp2Dd41-Rw/com.ibm.team.workitem.workItemType.task
		/// </summary>
		/// <value>
		/// The type resource.
		/// </value>
		public string TypeResource { get; set; }
	}
}