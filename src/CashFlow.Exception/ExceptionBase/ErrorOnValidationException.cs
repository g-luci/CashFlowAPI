namespace CashFlow.Exception.ExceptionBase
{
    public class ErrorOnValidationException : CashFlowException
    {
        public List<string> Errors { get; set; }
        public ErrorOnValidationException(List<string> errorMessage) : base(string.Empty)
        {
            Errors = errorMessage;
        }
    }
}
