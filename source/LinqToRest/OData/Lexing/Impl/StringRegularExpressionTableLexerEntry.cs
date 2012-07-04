namespace LinqToRest.OData.Lexing.Impl
{
	public class StringRegularExpressionTableLexerEntry : AbstractRegularExpressionTableLexerEntry
	{
		public override TokenType TokenType { get { return TokenType.String; } }

		public StringRegularExpressionTableLexerEntry() : base(@"'(?:[^'\\]*(?:\\.[^'\\]*)*)'")
		{
		}
	}
}