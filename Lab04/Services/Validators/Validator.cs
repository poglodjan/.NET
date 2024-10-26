using System;
using System.Text.RegularExpressions;

namespace Lab04.Services.Validators
{
    public abstract class Validator
    {
        public abstract bool Validate(string? valString);
    }

    public class EmailAddressValidator : Validator
    {
        public override bool Validate(string? valString)
        {
            if (string.IsNullOrEmpty(valString))
                return false;
            var isValid = valString.Contains("@") && valString.EndsWith(".com");
            return isValid;
        }
    }

    public class NameValidator : Validator
    {
        public override bool Validate(string? valString)
        {
            if (string.IsNullOrEmpty(valString))
                return false;
            var isValid = Regex.IsMatch(valString, @"^[a-zA-Z\s]+$");
            return isValid;
        }
    }

    public class PhoneNumberValidator : Validator
    {
        public override bool Validate(string? valString)
        {
            if (string.IsNullOrEmpty(valString))
                return false;
            var isValid = Regex.IsMatch(valString, @"^\d{9}$");
            return isValid;
        }
    }


}
