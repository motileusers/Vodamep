using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using System.Diagnostics;

namespace Vodamep.Data
{


    /// <summary>
    /// Werte der Code Provider auslesen
    /// </summary>
    public class CodeProviderValueEnumerator
    {

        public void Enumerate()
        {


            List<CodeProviderValue> result = new List<CodeProviderValue>();


            // Auflisten der der Daten aus den Code Providern
            // Alle Texte in ein Dictionary mit Keys schreiben

            List<Type> codeProviderTypes = Assembly.GetAssembly(typeof(CodeProviderBase))
                                                   .GetTypes()
                                                   .Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(CodeProviderBase)))
                                                   .ToList();

            Dictionary<string, string> textDictionary = new Dictionary<string, string>();

            foreach (Type codeProviderType in codeProviderTypes)
            {
                // das sind keine Enum Provider, sonder nur Listen von Länder, Orten, Versicherungen
                // da gibts nur ungewollte Überschneidungen, die lassen wir weg
                if (codeProviderType != typeof(CountryCodeProvider) &&
                    codeProviderType != typeof(Postcode_CityProvider) &&
                    codeProviderType != typeof(Vodamep.Data.Hkpv.Postcode_CityProvider) &&
                    codeProviderType != typeof(InsuranceCodeProvider))
                {
                    CodeProviderBase baseProvider = Activator.CreateInstance(codeProviderType) as CodeProviderBase;

                    IReadOnlyDictionary<string, string> values = baseProvider.Values;

                    foreach (KeyValuePair<string, string> key in values)
                    {
                        if (!textDictionary.ContainsKey(key.Key))
                        {
                            textDictionary.Add(key.Key, key.Value);
                        }
                        else
                        {
                            // doppelte Keys, das ist nicht so schön
                            // potenzielles Problem, dass ein Key in einer anderen Auflistung anders heißt
                        }
                    }
                }

            }



            // Auflisten der Enums aus den Proto Definitionen
            // Mapping mit den Keys aus den Daten vom Code Provider

            List<Type> reflectionTypes = Assembly.GetAssembly(typeof(CodeProviderBase))
                                  .GetTypes()
                                  .Where(p => p.Name.EndsWith("Reflection"))
                                  .ToList();



            foreach (Type messageType in reflectionTypes)
            {
                PropertyInfo[] properties = messageType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == "Descriptor")
                    {
                        FileDescriptor fileDescriptor = property.GetValue(messageType, null) as FileDescriptor;
                        IList<EnumDescriptor> enumDescriptors = fileDescriptor.EnumTypes;

                        foreach (EnumDescriptor enumDescriptor in enumDescriptors)
                        {
                            // Für die hier gibt's eine Beschreibungstexte
                            if (enumDescriptor.ClrType != typeof(Vodamep.Hkpv.Model.ActivityType))
                            {

                                Type enumClrType = enumDescriptor.ClrType;
                                List<string> names = Enum.GetNames(enumClrType).ToList();


                                IList<EnumValueDescriptor> enumDescriptorValues = enumDescriptor.Values;
                                foreach (EnumValueDescriptor enumDescriptorValue in enumDescriptorValues)
                                {

                                    CodeProviderValue codeProviderValue = new CodeProviderValue()
                                    {
                                        ReportType = messageType,
                                        EnumType = enumClrType,
                                        EnumValue = names[enumDescriptorValue.Index],
                                        ProtoValue = enumDescriptorValue.Name,
                                        Text = "",
                                    };

                                    if (textDictionary.ContainsKey(codeProviderValue.ProtoValue))
                                    {
                                        codeProviderValue.Text = textDictionary[codeProviderValue.ProtoValue];
                                    }
                                    else
                                    {
                                        throw new Exception("Keinen Wert im csv gefunden.");
                                    }

                                    result.Add(codeProviderValue);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
