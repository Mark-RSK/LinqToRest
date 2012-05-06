using LinqToRest.OData.Filters;

namespace LinqToRest.OData
{
	public class FilterQueryPart : ODataQueryPart
	{
		public override ODataQueryPartType QueryPartType { get { return ODataQueryPartType.Filter; } }

		public ODataQueryFilterExpression FilterExpression { get; private set; }

		public FilterQueryPart(ODataQueryFilterExpression filterExpression)
		{
			FilterExpression = filterExpression;
		}

		public override string ToString()
		{
			var result = BuildParameterString(FilterExpression.ToString());

			return result;
		}
	}
}