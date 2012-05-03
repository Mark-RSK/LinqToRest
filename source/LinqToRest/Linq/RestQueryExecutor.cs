using System;
using System.Collections.Generic;
using System.Linq;

using LinqToRest.Http;
using LinqToRest.Serialization;

using Remotion.Linq;

namespace LinqToRest.Linq
{
	public class RestQueryExecutor : IQueryExecutor
	{
		private readonly RestQueryModelVisitor _queryModelVisitor = new RestQueryModelVisitor();

		private readonly IHttpService _httpService;
		private readonly ISerializer _serializer;

		public RestQueryExecutor() : this(DependencyResolver.Current.GetInstance<IHttpService>(),
											DependencyResolver.Current.GetInstance<ISerializer>())
		{
		}

		public RestQueryExecutor(IHttpService httpService, ISerializer serializer)
		{
			_httpService = httpService;
			_serializer = serializer;
		}

		private string GetResult(Uri uri)
		{
			var json = _httpService.Get(uri);

			return json;
		}

		public T ExecuteScalar<T>(QueryModel queryModel)
		{
			// TODO: is this even right?
			return ExecuteSingle<T>(queryModel, false);
		}

		public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
		{
			var data = ExecuteCollection<T>(queryModel);

			var result = data.SingleOrDefault();

			if ((returnDefaultWhenEmpty == false) && (data == null))
			{
				throw new InvalidOperationException("Sequence contains no elements.");
			}

			return result;
		}

		public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
		{
			var url = _queryModelVisitor.Translate(queryModel);

			var uri = new Uri(url);

			var data = GetResult(uri);

			return _serializer.Deserialize<IEnumerable<T>>(data);
		}
	}
}