using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace bioscoop_app.Helper
{
    public class Validator
    {
        /// <summary>
        /// Check given email is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public static bool IsEmail(string email)
        {
            var emailAttr = new EmailAddressAttribute();
            return emailAttr.IsValid(email);
        }

        /// <summary>
        /// Check given name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsName(string name)
        {
            Regex nameRegex = new Regex("/^[a-z ,.'-]+$/i");
            return nameRegex.IsMatch(name);
        }
    }
}