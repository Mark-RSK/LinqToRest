using System;

namespace LinqToRest.OData.Filters
{
	public class UnaryFilterExpression : FilterExpression, IEquatable<UnaryFilterExpression>
	{
		public override FilterExpressionType ExpressionType { get { return FilterExpressionType.Unary; } }

		public FilterExpressionOperator Operator { get; private set; }

		public FilterExpression Operand { get; private set; }

		public UnaryFilterExpression(FilterExpressionOperator op, FilterExpression operand)
		{
			if (op == FilterExpressionOperator.Unknown)
			{
				throw new ArgumentException("Cannot create UnaryFilterExpression with operator type 'Unknown'.");
			}

			if (operand == null)
			{
				throw new ArgumentNullException("operand", "Operand cannot be null.");
			}

			Operator = op;
			Operand = operand;
		}

		public bool Equals(UnaryFilterExpression other)
		{
			var result = false;

			if (ReferenceEquals(null, other))
			{
				result = false;
			}
			else if (ReferenceEquals(this, other))
			{
				result = true;
			}
			else
			{
				result = Equals(other.Operator, Operator) && Equals(other.Operand, Operand);
			}

			return result;
		}

		public override bool Equals(object obj)
		{
			var result = false;

			if (ReferenceEquals(null, obj))
			{
				result = false;
			}
			else if (ReferenceEquals(this, obj))
			{
				result = true;
			}
			else if (obj.GetType() != typeof (UnaryFilterExpression))
			{
				result = false;
			}
			else
			{
				result = Equals((UnaryFilterExpression) obj);
			}
			return result;
		}

		public override int GetHashCode()
		{
			var result = String.Format("Operator:{0};Operand:{1};", Operator, Operand);

			return result.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0}({1}))", Operator.GetODataQueryOperatorString(), Operand);
		}
	}
}
