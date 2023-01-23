using Google.Protobuf.Reflection;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vodamep
{

    /// <summary>
    /// Gesamte Message auf gültige Werte prüfen
    /// </summary>
    /// <remarks>
    /// Enum Prüfung:
    /// - in Protobuf können sowohl int-Indizes wie Text Keys als Enum Werte angegeben werden
    /// - im Feld steht dann z.B. 1 statt MaleGe
    /// - das zieht sich dann durch alle Prüfugnen
    /// - wir erlauben nur die Angabe des Enum-Textes (MaleGe, bzw. MALE_GE)
    /// </remarks>
    public class MessageValuesValidator
    {

        /// <summary>
        /// Dictionary zum Zwischenspeichern der Enum Werte
        /// </summary>
        static Dictionary<Type, Dictionary<string, string>> enumValuesDictionary = new Dictionary<Type, Dictionary<string, string>>();


        /// <summary>
        /// Alle Werte in der in der Message iterieren
        /// </summary>
        public static void CheckMessageValues(IMessage protobufMessage)
        {
            if (protobufMessage == null)
            {
                return;
            }
            foreach (var field in protobufMessage.Descriptor.Fields.InFieldNumberOrder())
            {
                if (field.IsMap)
                {
                    var valueField = field.MessageType.Fields.InFieldNumberOrder()[1];
                    IDictionary mapFields = field.Accessor.GetValue(protobufMessage) as IDictionary;
                    if (mapFields != null)
                    {
                        foreach (DictionaryEntry mapField in mapFields)
                        {
                            if (valueField.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                            {
                                CheckMessageValues(mapField.Value as IMessage);
                            }
                            else
                            {
                                CheckEnumValue(valueField, mapField.Value);
                            }
                        }
                    }
                }
                else if (field.IsRepeated)
                {
                    IList repeatedFieldValues = field.Accessor.GetValue(protobufMessage) as IList;
                    if (repeatedFieldValues != null)
                    {
                        foreach (var repeatedFieldValue in repeatedFieldValues)
                        {
                            // for repeated fields, the field type represents each element in the list, handle them accordingly
                            if (field.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                            {
                                CheckMessageValues(repeatedFieldValue as IMessage);
                            }
                            else
                            {
                                CheckEnumValue(field, repeatedFieldValue);
                            }
                        }
                    }
                }
                else
                {
                    if (field.FieldType == Google.Protobuf.Reflection.FieldType.Message)
                    {
                        var fieldValue = field.Accessor.GetValue(protobufMessage);
                        CheckMessageValues(fieldValue as IMessage);
                    }
                    else
                    {
                        CheckFieldValue(field, protobufMessage);
                    }
                }
            }
        }


        /// <summary>
        /// Einzelnen Wert prüfen
        /// </summary>
        private static void CheckFieldValue(FieldDescriptor field, IMessage protobufMessage)
        {
            var fieldValue = field.Accessor.GetValue(protobufMessage);

            CheckEnumValue(field, fieldValue);
        }


        /// <summary>
        /// Enum Prüfung durchführen
        /// </summary>
        private static void CheckEnumValue(FieldDescriptor field, object fieldValue)
        {
            if (fieldValue != null)
            {
                if (field.FieldType == FieldType.Enum)
                {
                    EnumDescriptor enumDescriptor = field.EnumType;
                    Type t = enumDescriptor.ClrType;

                    if (!enumValuesDictionary.ContainsKey(t))
                    {
                        Dictionary<string, string> dictToAdd = Enum.GetValues(t).Cast<object>().ToDictionary(v => v.ToString(), v => v.ToString());
                        enumValuesDictionary.Add(t, dictToAdd);
                    }

                    Dictionary<string, string> valuesDictionary = enumValuesDictionary[t];
                    if (!valuesDictionary.ContainsKey(fieldValue.ToString()))
                    {
                        throw new Exception($"Value {fieldValue} not allowed for enum field {field.Name}");
                    }
                }
            }
        }
    }
}
