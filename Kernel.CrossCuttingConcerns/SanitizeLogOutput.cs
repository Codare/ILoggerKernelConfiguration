using System;
using System.Security.Cryptography;
using System.Text;

namespace Kernel.CrossCuttingConcerns
{
    public static class SanitizeLogOutput
    {
        public static string GetHashedString(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(stringToHash));
                return Convert.ToBase64String(hash);
            }
        }

        public static string RedactSensitiveInfo(string value)
        {
            if (value == null)
            {
                return null;
            }

            string[] parts = value.Split('/');
            foreach (string part in parts)
                if (Guid.TryParse(part, out _))
                    value = value.Replace(part, "GUID");
                else if (IsValidEmailAddress(part))
                    value = value.Replace(part, "EMAIL");
                else
                    value = GetHashedString(value);

            return value;
        }

        private static bool IsValidEmailAddress(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }
    }
}
