namespace PulsePeak.Core.Exceptions
{
    public class DbContextException : Exception
    {
        public DbContextException(string message) : base(message) { }

        public DbContextException(string message, Exception innerException) : base(message, innerException) { }
    }
}
