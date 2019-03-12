namespace Modules.Core.Contracts.Transport
{
	using System;
	using System.ServiceModel.Configuration;

	using JetBrains.Annotations;

	[UsedImplicitly]
    public sealed class LocaleBehaviourElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior() => new LocaleBehaviour();

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>
        /// The type of behavior.
        /// </returns>
        public override Type BehaviorType => typeof(LocaleBehaviour);
    }
}