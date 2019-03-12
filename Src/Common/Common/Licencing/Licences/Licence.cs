namespace Common.Licencing.Licences
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	internal abstract class Licence : ILicence
	{
		private readonly IEnumerable<ILicenceComponent> _components;

		public abstract string Description { get; }

		public abstract string Id { get; }

		protected Licence([NotNull] IEnumerable<ILicenceComponent> components)
		{
			if (components == null) throw new ArgumentNullException(nameof(components));

			_components = components;
		}

		public TComponent Get<TComponent>() where TComponent : ILicenceComponent =>
			_components
				.Where(_ => _ is TComponent)
				.Cast<TComponent>()
				.SingleOrDefault();

		public IDictionary<string, string> GetCapabilities()
		{
			var result = new Dictionary<string, string>();

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var component in _components)
			{
				foreach (var capability in component.GetCapabilities())
				{
					result.Add(capability.Key, capability.Value);
				}
			}

			return result;
		}
	}
}