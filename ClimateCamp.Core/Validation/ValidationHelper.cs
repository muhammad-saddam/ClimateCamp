using Abp.Extensions;
using PhoneNumbers;
using System;
using System.Text.RegularExpressions;

namespace ClimateCamp.Common.Validation
{
    public static class ValidationHelper
    {
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public static bool IsEmail(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            var regex = new Regex(EmailRegex);
            return regex.IsMatch(value);
        }

        public static bool IsValidPhone(string phoneNumber, string countryCode = null)
        {
            if (phoneNumber.IsNullOrWhiteSpace())
            {
                return true; //phone number is not required
            }

            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber checkPhoneNumber = phoneUtil.Parse(phoneNumber, countryCode);

                bool isMobile = false;
                bool isValidNumber = phoneUtil.IsValidNumber(checkPhoneNumber); // returns true for valid number

                return isValidNumber;
            }

            catch (Exception ex)
            {
                return false;
            }

        }
    }


}
