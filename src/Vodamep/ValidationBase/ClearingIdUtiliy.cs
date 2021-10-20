using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Vodamep.ValidationBase
{
    public class ClearingIdUtiliy
    {
        public static string CreateClearingPersonId(string family, string given, DateTime birthday)
        {
            string birthdayFormatted = birthday.ToString("ddMMyyyy");
            string personId = family + "." + given + "." + birthdayFormatted;

            personId = personId.ToLower();
            personId = personId.Replace(" ", "");
            personId = personId.Replace(" ", "");

            return personId;

        }

        public static string GetPersonHash(string personId, string secret)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(personId));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
