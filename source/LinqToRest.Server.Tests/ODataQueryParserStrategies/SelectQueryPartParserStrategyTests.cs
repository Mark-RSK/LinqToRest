// ReSharper disable InconsistentNaming

using LinqToRest.OData;
using LinqToRest.Server.OData.Parsing;
using LinqToRest.Server.OData.Parsing.Impl;

using NUnit.Framework;

namespace LinqToRest.Server.Tests.ODataQueryParserStrategies
{
	[TestFixture]
	public class SelectQueryPartParserStrategyTests
	{
		private const ODataQueryPartType Type = ODataQueryPartType.Select;
		private readonly IODataQueryPartParserStrategy _strategy = new SelectQueryPartParserStrategy();

		[Test]
		public void Parse_IncorrectType_ThrowsArgumentException()
		{
			Assert.That(() => _strategy.Parse(ODataQueryPartType.Ordering, "TestProperty, TestString"), Throws.ArgumentException);
		}

		[Test]
		public void Parse_PropertySelectPredicate_ReturnsCorrectQueryPart()
		{
			var result = _strategy.Parse(Type, "TestProperty, TestString");

			Assert.That(result, Is.InstanceOf<SelectQueryPart>());
			Assert.That(result.ToString(), Is.EqualTo("$select=TestProperty, TestString"));
		}
	}
}
