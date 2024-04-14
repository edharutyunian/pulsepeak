using System.Diagnostics;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
namespace PulsePeak.Core.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Retrieves a formatted exception message containing class, method, and error details.
        /// </summary>
        /// <param name="exception">The exception for which to retrieve the class name, method name and error message.</param>
        /// <returns>
        /// A string containing information about the class, method, and error message of the provided exception.
        /// </returns>
        /// <remarks>
        /// This method creates a formatted exception message including the class name, method name, and the error message
        /// from the specified exception object.
        /// </remarks>
        /// <param name="exception">The exception for which to retrieve the message.</param>
        public static string GetFormattedExceptionDetails(Exception exception)
        {
            var trace = new StackTrace(exception, true);
            string className = Path.GetFileNameWithoutExtension(trace.GetFrame(0).GetFileName());
            string methodName = trace.GetFrame(0).GetMethod().Name;

            return $"Class: {className} | Method: {methodName} | Error Message: {exception.Message}";
        }
        /// <summary>
        /// Retrieves a formatted exception message containing class, method, and error details and additional error message if any.
        /// </summary>
        /// <param name="exception">The exception for which to retrieve the class name, method name and error message.</param>
        /// <param name="additionalErrorMessage">The additional exception message if any.</param>
        /// <returns>
        /// A string containing information about the class, method, and error message of the provided exception.
        /// </returns>
        /// <remarks>
        /// This method creates a formatted exception message including the class name, method name, error message, and the additional error message
        /// from the specified exception object.
        /// </remarks>
        /// <param name="exception">The exception for which to retrieve the message.</param>
        public static string GetFormattedExceptionDetails(Exception exception, string additionalErrorMessage)
        {
            var trace = new StackTrace(exception, true);
            string className = Path.GetFileNameWithoutExtension(trace.GetFrame(0).GetFileName());
            string methodName = trace.GetFrame(0).GetMethod().Name;

            return $"Class: {className} | Function: {methodName} | Error Message: {additionalErrorMessage} {exception.Message}";
        }

        /// <summary>
        /// Validates that the specified required properties in the given object are not null or empty.
        /// </summary>
        /// <param name="requiredFields">A list of property names that are required.</param>
        /// <param name="obj">The object to validate.</param>
        /// <remarks>
        /// This method checks each specified required property in the object to ensure it is not null or empty.
        /// If a required property is found to be null or empty, an <see cref="ArgumentException"/> is thrown.
        /// </remarks>
        public static void ValidateRequiredFields(ICollection<string> requiredFields, object obj)
        {
            foreach (var fieldName in requiredFields) {
                var fieldValue = GetFieldValue(fieldName, obj);
                if (fieldValue == null) {
                    throw new ArgumentException($"{fieldName} is required, but the property does not exist or has a case-sensitive typo.");
                }
                if (fieldValue == null || (fieldValue is string strValue && string.IsNullOrWhiteSpace(strValue))) {
                    throw new ArgumentException($"{fieldName} is required and cannot be null or empty.");
                }
            }
        }

        /// <summary>
        /// Retrieves the value of the specified field from an object.
        /// </summary>
        /// <param name="fieldName">The name of the field to retrieve.</param>
        /// <param name="obj">The object from which to retrieve the field value.</param>
        /// <returns>
        /// The value of the specified field, or null if the field does not exist.
        /// </returns>
        public static object GetFieldValue(string fieldName, object obj)
        {
            return obj.GetType().GetProperty(fieldName)?.GetValue(obj);
        }
    }
}