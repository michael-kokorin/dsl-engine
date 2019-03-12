namespace Infrastructure.Reports.Blocks.Iterator
{
	using System;

	internal sealed class EmptyIteratorTemplateException : Exception
	{
		public EmptyIteratorTemplateException(IteratorReportBlock iteratorReportBlock)
			: base("Empty iterator template.")
		{

		}
	}
}