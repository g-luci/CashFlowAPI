namespace CashFlow.Exception.ExceptionBase
{
    public abstract class CashFlowException : SystemException
    {
        protected CashFlowException(string message) : base(message)
        {
            
        }
    }
}
