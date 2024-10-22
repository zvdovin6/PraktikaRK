using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Todo
{
    internal class InputValidator
    {
        public bool IsValidEmail(string email)
        {

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }


        public bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }


        public bool IsValidName(string name)
        {
            return name.Length >= 3;
        }
    }
}
