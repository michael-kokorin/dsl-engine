namespace Common.Logging
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	using JetBrains.Annotations;

	using Newtonsoft.Json;

	using PostSharp.Aspects;

	using Common.Container;
	using Common.Extensions;
	using Common.Properties;
	using Common.Time;

	using static Properties.Resources;

	/// <summary>
	///   Allows to log method call and input and output parameters. Exceptions thrown by method are logged also.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public sealed class LogMethodAttribute: MethodInterceptionAspect
	{
		[NonSerialized]
		private ILog _logger;

		/// <summary>
		///   Initializes a new instance of the <see cref="LogMethodAttribute"/> class.
		/// </summary>
		public LogMethodAttribute()
		{
			LogMeasure = true;

			MaxExecutionTime = 200;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="LogMethodAttribute"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public LogMethodAttribute(ILog logger)
			: this()
		{
			_logger = logger;
		}

		/// <summary>
		///   Gets or sets a value indicating whether all levels of properties should be logged.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if all levels of properties should be logged; otherwise, <see langword="false"/> and
		///   then the first level of properties with primitive types will be logged only.
		/// </value>
		public bool DeepLogging { get; set; }

		/// <summary>
		///   Gets or sets a value indicating whether input parameters should be logged.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if input parameters should be logged; otherwise, <see langword="false"/>.
		/// </value>
		public bool LogInputParameters { get; set; }

		/// <summary>
		///   Gets or sets a value indicating whether [log measure].
		/// </summary>
		/// <value>
		///   <c>true</c> if [log measure]; otherwise, <c>false</c>.
		/// </value>
		public bool LogMeasure { get; set; }

		/// <summary>
		///   Gets or sets a value indicating whether output value should be logged.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if output value should be logged; otherwise, <see langword="false"/>.
		/// </value>
		public bool LogOutputValue { get; set; }

		/// <summary>
		///   Gets or sets a value indicating whether only primitive input and output parameters should be logged.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if only primitive input and output parameters should be logged; otherwise,
		///   <see langword="false"/>.
		/// </value>
		public bool LogPrimitiveParametersOnly { get; set; }

		/// <summary>
		///   Gets or sets the maximal count of elements in collection parameters to log.
		/// </summary>
		/// <value>
		///   The maximal count of elements in collection parameters to log.
		/// </value>
		public int MaxArrayItemsToLog { get; set; } = 10;

		/// <summary>
		///   Gets or sets the maximum method execution time
		/// </summary>
		/// <value>
		///   Method maximum execution time.
		/// </value>
		public int MaxExecutionTime { get; set; }

		/// <summary>
		///   Gets or sets a value indicating whether json serialization should be used for input and output parameters.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if json serialization should be used; otherwise, <see langword="false"/>.
		/// </value>
		public bool UseJson { get; set; }

		/// <summary>
		///   Adds the collection to output result.
		/// </summary>
		/// <param name="builder">The output result builder.</param>
		/// <param name="parameterName">The name of parameter.</param>
		/// <param name="value">The value of parameter.</param>
		private void AddCollection(StringBuilder builder, string parameterName, ICollection value)
		{
			AddParameter(builder, CollectionParameterLength.FormatWith(parameterName), value.Count);

			var elementIndex = 0;
			foreach(var valueItem in value)
			{
				LogArgument(builder, CollectionElementName.FormatWith(parameterName, elementIndex), valueItem);

				elementIndex++;

				// if we reached out maximum - skip other elements in collection
				if(elementIndex >= Math.Min(value.Count, MaxArrayItemsToLog))
					break;
			}
		}

		/// <summary>
		///   Adds the parameter to output result.
		/// </summary>
		/// <param name="builder">The output result builder.</param>
		/// <param name="name">The name of parameter.</param>
		/// <param name="value">The value of parameter.</param>
		private static void AddParameter(StringBuilder builder, string name, object value)
			=> builder.AppendLine(ParameterNameValue.FormatWith(name, value ?? NullValue));

		/// <summary>
		///   Builds the name of the method.
		/// </summary>
		/// <param name="info">The method information.</param>
		/// <returns>The method name.</returns>
		private static string BuildMethodName(MethodBase info) =>
			MethodSignature.FormatWith(
				info.DeclaringType?.Name,
				info.Name,
				info.GetParameters().Select(x => ParameterName.FormatWith(x.ParameterType.Name, x.Name)).ToCommaSeparatedString());

		/// <summary>
		///   Determines whether the specified type is a collection.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns><see langword="true"/> when the specified type is a collection; otherwise, <see langword="false"/>.</returns>
		private static bool IsCollection(Type type)
			=> type.IsArray || type.GetInterfaces().Contains(typeof(ICollection));

		/// <summary>
		///   Determines whether the specified type is primitive.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns><see langword="true"/> when the specified type is primitive; otherwise, <see langword="false"/>.</returns>
		private static bool IsPrimitiveType(Type type) => type.IsPrimitive || type.IsEnum || (type == typeof(string));

		/// <summary>
		///   Logs the argument to the result.
		/// </summary>
		/// <param name="builder">The output result builder.</param>
		/// <param name="name">The name of parameter.</param>
		/// <param name="value">The value of parameter.</param>
		private void LogArgument(StringBuilder builder, string name, object value)
		{
			if(value == null)
			{
				AddParameter(builder, name, NullValue);
				return;
			}

			var type = value.GetType();

			if(IsPrimitiveType(type))
			{
				AddParameter(builder, name, value);
				return;
			}

			if(LogPrimitiveParametersOnly)
				return;

			if(IsCollection(type))
			{
				AddCollection(builder, name, value as ICollection);
				return;
			}

			foreach(
				var propertyInfo in
					value.GetType()
							.GetProperties()
							.WhereIf(!DeepLogging, x => IsPrimitiveType(x.PropertyType) || IsCollection(x.PropertyType)))
				LogArgument(builder, ParameterName.FormatWith(name, propertyInfo.Name), propertyInfo.GetValue(value));
		}

		/// <summary>
		///   Adds parameter to the output result.
		/// </summary>
		/// <param name="value">The value to add.</param>
		/// <returns>The serialized parameter value using custom serialization.</returns>
		private string LogArgument(object value)
		{
			var builder = new StringBuilder();
			LogArgument(builder, ReturnValueName, value);

			return builder.ToString();
		}

		/// <summary>
		///   Logs parameter values using custom text serialization.
		/// </summary>
		/// <param name="args">The parameters info.</param>
		/// <returns>Serialized values of parameters.</returns>
		private string LogCustom(MethodInterceptionArgs args)
		{
			var builder = new StringBuilder();
			var argumentIndex = 0;
			foreach(var parameter in args.Method.GetParameters())
			{
				LogArgument(builder, parameter.Name, args.Arguments[argumentIndex]);
				argumentIndex++;
			}

			return builder.ToString();
		}

		/// <summary>
		///   Logs parameter values using json serialization.
		/// </summary>
		/// <param name="args">The parameters info.</param>
		/// <returns>Serialized values of parameters.</returns>
		private string LogJson(MethodInterceptionArgs args) =>
			args.Method.GetParameters()
					.WhereIf(LogPrimitiveParametersOnly, x => x.ParameterType.IsPrimitive || x.ParameterType.IsEnum)
					.Select((x, index) => ParameterNameValue.FormatWith(x.Name, JsonConvert.SerializeObject(args.Arguments[index])))
					.ToCommaSeparatedString();

		private void LogMethodMeasure(string methodName, TimeSpan timeSpan)
		{
			var infoString = ExecutionTimeInfo.FormatWith(MaxExecutionTime, timeSpan.TotalMilliseconds, methodName);

			if(timeSpan.TotalMilliseconds > MaxExecutionTime)
				_logger.Warning(infoString);
			else
				_logger.Trace(infoString);
		}

		/// <summary>
		///   Called when intercepting method is invoked.
		/// </summary>
		/// <param name="args">The interception arguments.</param>
		public override void OnInvoke(MethodInterceptionArgs args)
		{
			// skip all logging if it is turned off
			if(ConfigurationManager.AppSettings[Settings.Default.EnableLoggerSettingName]?.EqualIgnoreCase(false.ToString()) ??
				false)
			{
				base.OnInvoke(args);
				return;
			}

			var methodName = BuildMethodName(args.Method);

			if(_logger == null)
				_logger = IoC.Resolve<ILog>();

			if(LogInputParameters)
			{
				var parameters = UseJson ? LogJson(args) : LogCustom(args);

				_logger.Trace(MethodWithParameters.FormatWith(methodName, parameters));
			}
			else
				_logger.Trace(methodName);

			try
			{
				if(LogMeasure)
				{
					using(TimeMeasurement.Measure(_ => LogMethodMeasure(methodName, _)))
						base.OnInvoke(args);
				}
				else
					base.OnInvoke(args);

				if(!LogOutputValue)
					return;

				var output = LogArgument(args.ReturnValue);

				_logger.Trace(output);
			}
			catch(Exception exception)
			{
				_logger.Error(methodName, exception);
				throw;
			}
		}
	}
}