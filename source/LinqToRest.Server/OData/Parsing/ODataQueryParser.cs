using System;
using System.Linq;
using System.Text.RegularExpressions;

using LinqToRest.OData;

namespace LinqToRest.Server.OData.Parsing
{
	public class ODataQueryParser
	{
		private readonly IODataQueryPartParserStrategy _strategy;

		public ODataQueryParser(IODataQueryPartParserStrategy strategy)
		{
			_strategy = strategy;
		}

		public ODataQuery Parse(Uri query)
		{
			var result = new ODataQuery();

			var queryString = Uri.UnescapeDataString(query.Query);

			//var matches = Regex.Matches(queryString, @"[?&]([^=]+)=([^&]+)").Cast<Match>();
			var matches = Regex.Matches(queryString, @"[?&]([^=&]+)(?:=([^&]+))?").Cast<Match>();

			foreach (var match in matches)
			{
				var groups = match.Groups.Cast<Group>().Skip(1).ToList();

				var parameterName = groups[0].Value;
				var parameterValue = groups[1].Value;

				var queryPartType = parameterName.GetFromUrlParameterName();

				var parsedPart = _strategy.Parse(queryPartType, parameterValue);

				switch (queryPartType)
				{
					case ODataQueryPartType.Count:
						result.CountPredicate = (CountQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Expand:
						result.ExpandPredicate = (ExpandQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Filter:
						result.FilterPredicate = (FilterQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Format:
						result.FormatPredicate = (FormatQueryPart) parsedPart;
						break;
					case ODataQueryPartType.InlineCount:
						result.InlineCountPredicate = (InlineCountQueryPart) parsedPart;
						break;
					case ODataQueryPartType.OrderBy:
						result.OrderByPredicate = (OrderByQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Select:
						result.SelectPredicate = (SelectQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Skip:
						result.SkipPredicate = (SkipQueryPart) parsedPart;
						break;
					case ODataQueryPartType.SkipToken:
						result.SkipTokenPredicate = (SkipTokenQueryPart) parsedPart;
						break;
					case ODataQueryPartType.Top:
						result.TopPredicate = (TopQueryPart) parsedPart;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return result;
		}
	}
}
