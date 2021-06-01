using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Vodamep.Data.Dummy
{
    internal class GeneratorBase
    {



        protected long _id = 1;
        protected Random _rand = new Random();
        protected string[] _addresses;
        protected string[] _names;
        protected string[] _familynames;
        protected string[] _activities;




        public GeneratorBase()
        {
            _addresses = ReadRessource("gemplzstr_8.csv").ToArray();
            _names = ReadRessource("Vornamen.txt").ToArray();
            _familynames = ReadRessource("Nachnamen.txt").ToArray();
            _activities = ReadRessource("Aktivitäten.txt").ToArray();
        }


        private IEnumerable<string> ReadRessource(string name)
        {
            var assembly = this.GetType().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Data.Dummy.{name}");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }



        /// <summary>
        /// Liefert ein zufälliges Land, ohne ZZ
        /// </summary>
        protected string GetRandomCountryCode()
        {
            string result = "";

            string[] keys = CountryCodeProvider.Instance.Values.Keys
                            .Where(a => a != "ZZ")
                            .ToArray();

            int rand = _rand.Next(keys.Count() - 1);

            if (keys.Count() <= rand)
            {
            }
            result = keys[rand];

            return result;
        }


        /// <summary>
        /// Liefert einen zufälligen Enum, ohne den Index 0 (= Undefined)
        /// </summary>
        protected object GetRandomEnum(Type t)
        {
            Array array = Enum.GetValues(t);
            int rand = _rand.Next(array.Length - 1);
            return array.GetValue(rand + 1);
        }

    }
}

