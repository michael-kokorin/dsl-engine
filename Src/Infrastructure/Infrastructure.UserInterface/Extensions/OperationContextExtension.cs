namespace Infrastructure.UserInterface.Extensions
{
	using System.ServiceModel;
	using System.ServiceModel.Channels;

	internal static class OperationContextExtension
	{
		public static string GetRemoteIp(this OperationContext operationContext)
		{
			var endpoint = GetEndpoint(operationContext);

			return endpoint?.Address;
		}

		public static int GetRemotePort(this OperationContext operationContext)
		{
			var endPoint = GetEndpoint(operationContext);

			return endPoint.Port;
		}

		private static RemoteEndpointMessageProperty GetEndpoint(OperationContext operationContext)
		{
			var prop = operationContext.IncomingMessageProperties;

			var endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

			return endpoint;
		}
	}
}