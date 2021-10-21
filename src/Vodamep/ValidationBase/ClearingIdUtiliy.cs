using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Vodamep.ValidationBase
{
    public class ClearingIdUtiliy
    {

        /// <summary>
        /// Clearing ID anhand von Namen und Geburtsdatum erstellen
        /// </summary>
        public static string CreateClearingId(string family, string given, DateTime birthday)
        {
            string birthdayFormatted = birthday.ToString("ddMMyyyy");
            string personId = family?.Trim() + "." + given?.Trim() + "." + birthdayFormatted;

            personId = personId.ToLower();
            personId = personId.Replace(" ", "");

            return personId;
        }


        /// <summary>
        /// Ausnahmen für die Clearing IDs anwenden
        /// </summary>
        public static string MapClearingId(ClearingExceptions clearingExceptions, string existingClearingId, string sourcSystemId, string personId)
        {
            string result = existingClearingId;

            if (clearingExceptions != null)
            {
                clearingExceptions.BuildClearingDictionaries();

                if (clearingExceptions.EqualDictionary != null)
                {
                    if (clearingExceptions.EqualDictionary.ContainsKey(existingClearingId))
                    {
                        result = clearingExceptions.EqualDictionary[existingClearingId].ToId;
                    }
                }

                if (clearingExceptions.SplitDictionary != null)
                {
                    string spiltId = sourcSystemId + "." + personId;
                    if (clearingExceptions.SplitDictionary.ContainsKey(spiltId))
                    {
                        result = clearingExceptions.SplitDictionary[spiltId].ToId;
                    }
                }
            }

            return result;
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
