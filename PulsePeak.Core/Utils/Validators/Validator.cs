namespace PulsePeak.Core.Utils.Validators
{
    /// <summary>
    /// Some general static validators.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates that a given DateTime value is a reasonable birth date.
        /// </summary>
        /// <param name="birthDate">The birth date to validate.</param>
        /// <param name="errorMessage">Outputs an error message if the validation fails.</param>
        /// <returns><c>true</c> if the birth date is valid; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Validation checks include ensuring that the birth date is not in the future and that it is no more than 120 years in the past.
        /// </remarks>
        public static bool IsValidBirthDate(DateTime birthDate, out string errorMessage)
        {
            errorMessage = string.Empty;
            var today = DateTime.Today;
            var oldestAllowedDate = today.AddYears(-80);

            if (birthDate > today) {
                errorMessage = "The birth date cannot be in the future.";
                return false;
            }
            else if (birthDate < oldestAllowedDate) {
                errorMessage = $"The birth date is too far in the past. Birth dates earlier than {oldestAllowedDate:yyyy-MM-dd} are not allowed.";
                return false;
            }
            return true;
        }
    }
}
