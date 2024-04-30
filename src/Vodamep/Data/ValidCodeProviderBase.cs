using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vodamep.Data
{

    public abstract class ValidCodeProviderBase
    {
        private static Regex _commentPattern = new Regex("//.*$");
        private IDictionary<string, string> _dict = new Dictionary<string, string>();

        protected ValidCodeProviderBase()
        {
            this.Init();
        }

        public abstract string Unknown { get; }

        public virtual bool IsValid(string code) => _dict.ContainsKey(code ?? string.Empty);


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
                    _dict.Add(values[0], values[1]);
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
                return this._dict[valueDescriptior.Name];
            }
            catch (Exception exception)
            {
                throw new Exception($"Error reading enum value {code}.", exception);
            }
        }



        public IEnumerable<string> GetCSV() => _dict.Select(x => $"{x.Key};{x.Value}");

        public IReadOnlyDictionary<string, string> Values => new ReadOnlyDictionary<string, string>(_dict);

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
