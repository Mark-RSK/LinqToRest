using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;

using LinqToRest.Server.OData;

namespace LinqToRest.Server.WebApi
{
	public class ODataQueryAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext == null)
			{
				throw new ArgumentNullException("actionExecutedContext");
			}

			if (actionExecutedContext.Request == null)
			{
				throw new ArgumentException("The HttpActionExecutedContext cannot contain a null HttpRequestMessage.");
			}

			var request = actionExecutedContext.Request;
			var response = actionExecutedContext.Response;

			IQueryable query;
			if ((response != null) && response.TryGetContentValue(out query))
			{
				if ((request.RequestUri != null) && (String.IsNullOrWhiteSpace(request.RequestUri.Query) == false))
				{
					var itemType = query.ElementType;

					var requestUri = request.RequestUri;

					var parser = new ODataUriParser();

					var expression = parser.Parse(itemType, requestUri);

					var fn = expression.Compile();

					var result = fn.DynamicInvoke(query);

					var oldContent = (ObjectContent) response.Content;

					var newContent = new ObjectContent(expression.ReturnType, result, oldContent.Formatter, oldContent.Headers.ContentType.MediaType);

					response.Content = newContent;
				}
			}
		}
	}
}