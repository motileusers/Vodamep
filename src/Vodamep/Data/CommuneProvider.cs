using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Vodamep.Data
{


    public sealed class CommuneProvider
    {
        private static volatile CommuneProvider instance;
        private static object syncRoot = new Object();
        private static Regex _commentPattern = new Regex("//.*$");
        private IDictionary<string, Commune> _dict = new Dictionary<string, Commune>();

        public static CommuneProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CommuneProvider();
                    }
                }

                return instance;
            }
        }

        private CommuneProvider()
        {
            this.Init();
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

                    var communeValues = line.Split(';');
                    var postCodeCityValues = communeValues[0].Split(' ');

                    Commune commune;

                    if (!this._dict.ContainsKey(communeValues[1]))
                    {
                        commune = new Commune()
                        {
                            Id = communeValues[1],
                            Name = postCodeCityValues[1]
                        };

                        this._dict.Add(commune.Id, commune);
                    }
                    else
                    {
                        commune = this._dict[communeValues[1]];
                    }


                    commune.PostcodeCities.Add(
                            new PostcodeCity()
                            {
                                PoCode = postCodeCityValues[0],
                                City = postCodeCityValues[1]
                            });

                }
            }
        }

        public IReadOnlyDictionary<string, Commune> Values => new ReadOnlyDictionary<string, Commune>(_dict);

        public bool IsValid(string code) => _dict.ContainsKey(code ?? string.Empty);


        string ResourceName => "Datasets.PostcodeCity.csv";
    }
}
