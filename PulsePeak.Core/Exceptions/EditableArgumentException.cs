namespace PulsePeak.Core.Exceptions
{
    public class EditableArgumentException : Exception
    {
        public EditableArgumentException(string message) : base(message) { }
        public EditableArgumentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
