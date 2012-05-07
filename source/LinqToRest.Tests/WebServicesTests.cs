﻿using System.Linq;

using LinqToRest.Tests.Mocks;

using NUnit.Framework;

namespace LinqToRest.Tests
{
	[TestFixture]
	public class WebServicesTests
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			DependencyResolver.SetDependencyResolver(new MockDependencyResolver());
		}

		[Test]
		public void Find_TypeWithoutServiceUrlAttributeSpecified_ThrowsArgumentException()
		{
			Assert.That(() => WebServices.Find<int>().ToList(), Throws.ArgumentException);
		}

		[Test]
		public void Find_TestObject_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>();

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json"));
		}

		[Test]
		public void Find_TestObjectSelectTestProperty_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>().Select(x => x.TestProperty);

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result.Select(x => x.TestProperty)));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$select=TestProperty"));
		}

		[Test]
		public void Find_TestObjectCount_GeneratesCorrectUrl()
		{
			var count = WebServices.Find<TestObject>().Count();

			Assert.That(count, Is.EqualTo(MockHttpService.Result.Count()));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$count"));
		}

		[Test]
		public void Find_TestObjectSkipThree_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>().Skip(3);

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result.Skip(3)));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$skip=3"));
		}

		[Test]
		public void Find_TestObjectOrderBy_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>().OrderBy(x => x.TestProperty);

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result.OrderBy(x => x.TestProperty)));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$orderby=TestProperty asc"));
		}

		[Test]
		public void Find_TestObjectTakeThree_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>().Take(3);

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result.Take(3)));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$top=3"));
		}

		[Test]
		public void Find_TestObjectWhereTestPropertyDoesNotEqualNull_GeneratesCorrectUrl()
		{
			var queryable = WebServices.Find<TestObject>().Where(x => x.TestProperty != null);

			var result = queryable.ToList();

			Assert.That(result, Is.EquivalentTo(MockHttpService.Result));

			var requestedUrl = MockHttpService.RequestedUrls.Pop();

			Assert.That(requestedUrl, Is.EqualTo("http://localhost/api/TestModel?$format=json&$filter=(TestProperty ne Null)"));
		}
	}
}
