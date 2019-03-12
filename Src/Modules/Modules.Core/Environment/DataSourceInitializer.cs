namespace Modules.Core.Environment
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Data;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceInitializer : IDataSourceInitializer
	{
		private readonly IQueryEntityNameRepository _queryEntityNameRepository;

		private readonly IUnityContainer _unityContainer;

		public DataSourceInitializer([NotNull] IUnityContainer unityContainer,
			[NotNull] IQueryEntityNameRepository queryEntityNameRepository)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));
			if (queryEntityNameRepository == null) throw new ArgumentNullException(nameof(queryEntityNameRepository));

			_unityContainer = unityContainer;
			_queryEntityNameRepository = queryEntityNameRepository;
		}

		/// <summary>
		///   Initializes data sources.
		/// </summary>
		public void Initialize() => RegisterEntityTypesNames();

		private void RegisterEntityTypesNames()
		{
			var sources = _unityContainer.ResolveAll<IDataSource>();

			if(sources == null)
				return;

			foreach(var source in sources)
			{
				var sourceType = source.GetType();

				var type = typeof(IDataSource<>);

				var interfaceType =
					sourceType.GetInterfaces()
							.Where(_ => _.IsGenericType && (_.GetGenericTypeDefinition() == type))
							.Select(_ => _.GetGenericArguments()[0])
							.SingleOrDefault();

				if(interfaceType == null)
					continue;

				var typeKey = interfaceType.Name;

				if(_queryEntityNameRepository.GetByKey(typeKey) != null)
					continue;

				_queryEntityNameRepository.Insert
				(
					new QueryEntityNames
					{
						AssemblyName = sourceType.Assembly.FullName,
						Key = typeKey,
						TypeName = interfaceType.FullName
					});

				_queryEntityNameRepository.Save();
			}
		}
	}
}