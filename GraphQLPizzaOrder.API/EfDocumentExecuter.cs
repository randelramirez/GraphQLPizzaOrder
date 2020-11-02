using GraphQL;
using GraphQL.Execution;
using GraphQLParser.AST;

namespace GraphQLPizzaOrder.API
{
    public class EFDocumentExecuter : DocumentExecuter
    {
        protected override IExecutionStrategy SelectExecutionStrategy(ExecutionContext context)
        {
            if (context.Operation.OperationType == GraphQL.Language.AST.OperationType.Query)
            {
                return new SerialExecutionStrategy();
            }
            return base.SelectExecutionStrategy(context);
        }
    }
}