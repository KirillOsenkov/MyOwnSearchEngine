using System;
using GuiLabs.MathParser;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine
{
    public class Math : IProcessor
    {
        public string GetResult(Query query)
        {
            var compiler = new Compiler();
            var result = compiler.CompileExpression(query.OriginalInput);
            if (result.IsSuccess)
            {
                double output;
                try
                {
                    output = result.Expression();
                }
                catch (Exception ex)
                {
                    return Div(ex.ToString());
                }

                return Div($"{output}");
            }

            return null;
        }
    }
}
