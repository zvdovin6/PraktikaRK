using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Todo
{
    public static class InputValidatorExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsValidPassword(this string password)
        {
            return password.Length >= 6;
        }

        public static bool IsValidName(this string name)
        {
            return name.Length >= 3;
        }

        public static bool ArePasswordsMatching(this string password, string confirmPassword)
        {
            return password == confirmPassword;
        }
    }
}
