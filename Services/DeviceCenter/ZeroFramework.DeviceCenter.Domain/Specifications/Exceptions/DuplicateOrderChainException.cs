namespace ZeroFramework.DeviceCenter.Domain.Specifications.Exceptions
{
    public class DuplicateOrderChainException : Exception
    {
        private const string message = "The specification contains more than one Order chain!";

        public DuplicateOrderChainException()
            : base(message)
        {
        }

        public DuplicateOrderChainException(Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
