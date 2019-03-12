namespace Infrastructure.Engines
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;

	using Common;
	using Common.Data;
	using Infrastructure.Engines.Query.Result;
	using Repository;

	public sealed class DataQueryExecutor : IDataQueryExecutor
	{
		private const string ClassContent = @"
namespace MyOwn
{
		using System.Collections;
		using System.Linq;

		using Common.Data;
		using Repository.Context;
		using Infrastructure.Engines;
	using Infrastructure.Engines.Query.Result;

		public static class Executor
		{
				public static object Execute(IDataSource<{type}> source)
				{
						return {query};
				}
		}
}";

		public object Execute<T>(IDataSource<T> source, string dataQuery)
			=> Execute(source as IDataSource<object>, typeof(T), dataQuery);

		public object Execute(IDataSource<object> source, Type entityType, string dataQuery)
		{
			var tree = GetSyntaxTree(entityType, dataQuery);

			var assemblyName = GetAssemblyName();

			var references = new []
			{
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
				MetadataReference.CreateFromFile(entityType.Assembly.Location),
				MetadataReference.CreateFromFile(typeof(QueryResultItem).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(CommonContainerModule).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(RepositoryContainerModule).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(EnginesContainerModule).Assembly.Location)
			};

			var compilation = CreateCompilation(assemblyName, tree, references);

			var stream = new MemoryStream();

			var result = compilation.Emit(stream);

			if (!result.Success)
			{
				var errors = result.Diagnostics
					.Select(x => new Tuple<string, string>(x.Id, x.GetMessage()));

				throw new DataQueryCompilationException(errors);
			}

			var executionResult = ExecuteAssembly(source, stream);

			return executionResult;
		}

		private static object ExecuteAssembly(IDataSource<object> source, MemoryStream stream)
		{
			var assembly = Assembly.Load(stream.ToArray());

			var type = assembly.GetType("MyOwn.Executor");

			var method = type.GetMethod("Execute");

			var executionResult = method.Invoke(null, new object[] {source});

			return executionResult;
		}

		private static CSharpCompilation CreateCompilation(string assemblyName, SyntaxTree tree, IEnumerable<PortableExecutableReference> references) =>
			CSharpCompilation.Create(
			assemblyName,
			new[] {tree},
			references,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		private static string GetAssemblyName() => Guid.NewGuid() + ".dll";

		private static SyntaxTree GetSyntaxTree(Type entityType, string dataQuery)
		{
			var text = ClassContent
				.Replace("{type}", entityType.FullName)
				.Replace("{query}", dataQuery);

			var tree = CSharpSyntaxTree.ParseText(text);
			return tree;
		}
	}
}