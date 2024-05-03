using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vodamep.Data
{

    public abstract class ValidCodeProviderBase
    {
        private static Regex _commentPattern = new Regex("//.*$");
        private IDictionary<string, ValidCode> _dict = new Dictionary<string, ValidCode>();
        private const string dateFormat = "dd.MM.yyyy";

        protected ValidCodeProviderBase()
        {
            this.Init();
        }

        public abstract string Unknown { get; }

        public virtual bool IsValid(string code, DateTime date)
        {
            if (String.IsNullOrWhiteSpace(code)) { return false; }
            if (!_dict.ContainsKey(code)) { return false; }

            ValidCode validCode = _dict[code];

            // z.B. Gültigkeit der Meldung 31.12.2022 < Start der Gültigkeit 01.01.2024
            if (date < validCode.ValidFrom) { return false; }

            // z.B. Gültigkeit der Meldung 01.01.2024 > Ende der Gültigkeit 31.12.2023
            if (date > validCode.ValidTo) { return false; }

            return true;
        }


        private void Init()
        {
            var assembly = this.GetType().Assembly;

            var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{this.ResourceName}");

            using (var reader = new StreamReader(resourceStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    line = _commentPattern.Replace(line, string.Empty).Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    var values = line.Split(';');

                    ValidCode validCode = new ValidCode();
                    validCode.Code = values[0];
                    validCode.Description = values[1];

                    if (values.Length > 2 &&
                        !String.IsNullOrWhiteSpace(values[2]))
                    {
                        validCode.ValidFrom = DateTime.ParseExact(values[2], dateFormat, CultureInfo.InvariantCulture);
                    }

                    if (values.Length > 3 &&
                        !String.IsNullOrWhiteSpace(values[3]))
                    {
                        validCode.ValidTo = DateTime.ParseExact(values[3], dateFormat, CultureInfo.InvariantCulture);
                    }

                    _dict.Add(validCode.Code, validCode);
                }
            }
        }

        /// <summary>
        /// Wert anhand eines CLR Enum Key ausgeben (im Dictionary sind die Proto-Keys hinterlegt)
        /// </summary>
        public string GetEnumValue(string code)
        {
            try
            {
                EnumDescriptor descriptor = this.Descriptor.EnumTypes.Where(x => x.Name == this.GetType().Name.Replace("Provider", "")).FirstOrDefault();
                Type clrType = descriptor?.ClrType;
                List<string> names = Enum.GetNames(clrType).ToList();
                int index = names.IndexOf(code);
                EnumValueDescriptor valueDescriptior = descriptor.Values[index];
                return this._dict[valueDescriptior.Name].Description;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error reading enum value {code}.", exception);
            }
        }



        public IEnumerable<string> GetCSV() => _dict.Select(x => $"{x.Key};{x.Value.Description}");

        public IReadOnlyDictionary<string, ValidCode> Values => new ReadOnlyDictionary<string, ValidCode>(_dict);

        protected abstract string ResourceName { get; }

        protected abstract FileDescriptor Descriptor { get; }


        /// <summary>
        /// Das sind Enum Provider
        /// Wenn der Protobuff Descriptor nicht gesetzt ist, handelt es sich um von Länder, Orten, Versicherungen, ...
        /// </summary>
        public bool IsEnumProvider
        {
            get
            {
                if (Descriptor != null)
                    return true;

                return false;
            }
        }


        internal static ValidCodeProviderBase GetInstance<T>()
            where T : ValidCodeProviderBase
        {
            ValidCodeProviderBase result = null;
            try
            {
                result = (ValidCodeProviderBase)typeof(T).GetProperty("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
            }
            catch
            {
                throw new System.Exception($"CodeProviderBase.GetInstance<{typeof(T).Name}> failed!");
            }

            return result;

        }
    }
}
