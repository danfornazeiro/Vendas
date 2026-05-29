namespace Vendas.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
            
        }

        public static void When(bool hasError, string erroMessage)
        {
            if (hasError)
            {
                throw new DomainException(erroMessage);
            }
        }
    }
}
