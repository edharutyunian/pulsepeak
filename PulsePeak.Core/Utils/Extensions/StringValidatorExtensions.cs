using System.Text.RegularExpressions;

namespace PulsePeak.Core.Utils.Extensions
{
    public static class StringValidatorExtensions
    {
        /// <summary>
        /// Validates whether the given name string meets specified criteria.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <param name="errorMessage">Outputs a descriptive error message if the validation fails.</param>
        /// <returns><c>true</c> if the name is valid according to the rules; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The name validation ensures the following:
        /// <para>- The name must start with a letter.</para>
        /// <para>- The name can only contain letters, spaces, apostrophes ('), dashes (-), and periods (.).</para>
        /// <para>- Special characters and spaces cannot occur at the beginning or end of the name.</para>
        /// <para>This method uses case-insensitive comparison.</para>
        /// </remarks>
        public static bool IsValidName(this string name, out string errorMessage)
        {
            errorMessage = string.Empty;
            string pattern = @"^(?=[A-Za-z])((?:[A-Za-z])|(?:[A-Za-z]['-.](?=[A-Za-z]))|(?: (?=[A-Za-z])))*$";

            if (!Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase)) {
                errorMessage = "The name must start with a letter and can only include letters, spaces, apostrophes, dashes, and periods.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates whether the given username meets the specified criteria.
        /// </summary>
        /// <param name="username">The username to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the username is invalid;
        /// otherwise, it will be an empty string.</param>
        /// <returns>
        ///   <c>true</c> if the username is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The username must:
        /// <para>- Be between 8 and 20 characters long.</para>
        /// <para>- Contain only alphanumeric characters, dots, and underscores.</para>
        /// <para>- Not start or end with a dot or underscore.</para>
        /// <para>- Not contain consecutive dots or underscores.</para>
        /// </remarks>
        public static bool IsValidUsername(this string username, out string errorMessage)
        {
            errorMessage = string.Empty;
            string pattern = @"^(?=[a-zA-Z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$";

            if (!Regex.IsMatch(username, pattern)) {
                errorMessage = "The username does not meet the specified criteria.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates whether the given phone number is a valid Armenian phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the phone number is invalid; 
        /// Otherwise, it will be an empty string.</param>
        /// <returns>
        ///   <c>true</c> if the phone number is a valid Armenian phone number; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Valid Armenian phone numbers may include:
        /// <para> - Optional leading '+' sign for international code.</para>
        /// <para> - Optional leading '374' for country code.</para>
        /// <para> - Optional parentheses, dots, or hyphens as separators.</para>
        /// <para> - Area code enclosed in parentheses is allowed.</para>
        /// <para> - Example formats: +374 (91) 404-101, 091.404.101, (091)-404-101.</para>
        /// </remarks>
        public static bool IsValidPhoneNumber(this string phoneNumber, out string errorMessage)
        {
            string intCodePhoneNumber = @"^\+?\d{3} *\(? *\.?\-?\d{2} *\)? *?\.?\-? *\d{3} *\(?\.?\-? *\d{3}$";
            string areaCodePhoneNumber = @"^ *\(?\.?\-? *\d{3}\)? *\.?\-? *\d{3} *\(?\.?\-? *\d{3}$";
            errorMessage = string.Empty;

            if (!Regex.IsMatch(phoneNumber, intCodePhoneNumber) && !Regex.IsMatch(phoneNumber, areaCodePhoneNumber)) {
                errorMessage = "The phone number is not a valid US or Canada phone number.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates whether the given email address is a valid email address with or without subdomains.
        /// </summary>
        /// <param name="emailAddress">The email address to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the email address is invalid; 
        /// otherwise, it will be an empty string.</param>
        /// <returns>
        ///   <c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The email address must adhere to standard email format rules. Two patterns are supported:
        /// <para> - Simple email pattern: example@domain.com </para> 
        /// <para> - Subdomain email pattern: example@sub.domain.com </para> 
        /// </remarks>
        public static bool IsValidEmailAddress(this string emailAddress, out string errorMessage)
        {
            string simpleEmailPattern = @"^[a-zA-Z0-9#!%$'&+*\/=?^_`.{|}~-]{0,61}[a-zA-Z0-9#!%$'&+*\/=?^_`.{|}~](@)[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$";
            string subEmailPattern = @"^[a-zA-Z0-9#!%$'&+*\/=?^_`.{|}~-]{0,61}[a-zA-Z0-9#!%$'&+*\/=?^_`.{|}~](@)[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+\.[a-zA-Z]{2,}$";
            errorMessage = string.Empty;

            if (!Regex.IsMatch(emailAddress, simpleEmailPattern) && !Regex.IsMatch(emailAddress, subEmailPattern)) {
                errorMessage = "The email address is not a valid email address.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates whether the given password meets the specified criteria for strength.
        /// </summary>
        /// <param name="password">The password to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the password is invalid; 
        /// otherwise, it will be an empty string.</param>
        /// <returns>
        ///     <c>true</c> if the password is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The password must meet the following criteria:
        /// <para> - At least 8 characters long.</para>
        /// <para> - Contains at least one lowercase letter.</para>
        /// <para> - Contains at least one uppercase letter.</para>
        /// <para> - Contains at least one digit.</para>
        /// <para> - Contains at least one special character.</para>
        /// </remarks>
        public static bool IsValidPassword(this string password, out string errorMessage)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            errorMessage = string.Empty;

            if (!Regex.IsMatch(password, pattern)) {
                errorMessage = "The password does not meet the requirements.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates whether the given string represents a valid ZIP Code in Armenia.
        /// </summary>
        /// <param name="zipCode">The ZIP Code to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the ZIP Code is invalid; 
        /// otherwise, it will be an empty string.</param>
        /// <returns>
        ///   <c>true</c> if the ZIP Code is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The ZIP Code must follow the standard Armenian ZIP Code format:
        /// <para>- Four digits.</para>
        /// <para>- Examples: 0010, 0011.</para>
        /// </remarks>
        public static bool IsValidZipCode(this string zipCode, out string errorMessage)
        {
            string pattern = @"^\d{4}?$";
            errorMessage = string.Empty;

            if (!Regex.IsMatch(zipCode, pattern)) {
                errorMessage = "Invalid ZIP Code.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates whether the given string represents a valid credit card number.
        /// </summary>
        /// <param name="creditCard">The credit card number to be validated.</param>
        /// <param name="errorMessage">An out parameter that will contain an error message if the credit card number is invalid; 
        /// otherwise, it will be an empty string.</param>
        /// <returns>
        ///   <c>true</c> if the credit card number is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The credit card number must follow standard validation patterns for major card issuers:
        /// <para>- Visa (starting with 4)</para>
        /// <para>- MasterCard (starting with 5)</para>
        /// <para>- American Express (starting with 34 or 37)</para>
        /// <para>- Discover (starting with 6011)</para>
        /// <para>- Diners Club (starting with 300-305, 36, or 38)</para>
        /// </remarks>
        public static bool IsValidCreditCardNumber(this string creditCard, out string errorMessage)
        {
            string pattern = @"(^4[0-9]{12}(?:[0-9]{3})?$)|(^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$)|(3[47][0-9]{13})|(^3(?:0[0-5]|[68][0-9])[0-9]{11}$)|(^6(?:011|5[0-9]{2})[0-9]{12}$)|(^(?:2131|1800|35\d{3})\d{11}$)";
            errorMessage = string.Empty;

            if (!Regex.IsMatch(creditCard, pattern)) {
                errorMessage = "Invalid credit card number.";
                return false;
            }
            return true;
        }
    }
}