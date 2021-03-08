using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.Mobile.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public string ErrorMessage { get; set; }

        public bool Validate(bool forRegister)
        {
            ErrorMessage = "";

            if (forRegister)
            {
                if (string.IsNullOrEmpty(RepeatPassword) || RepeatPassword != Password)
                    ErrorMessage = "Passwords do not match";
            }
            
            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
                ErrorMessage = "Password require minimum 6 characters";
            else if (Password.Contains(" "))
                ErrorMessage = "Password cannot include any empty space";

            if (string.IsNullOrEmpty(Email))
                ErrorMessage = "Enter your email";
            else if (Email.Length < 5 || !Email.Contains("@") || !Email.Contains("."))
                ErrorMessage = "Invalid email adress";

            return string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
