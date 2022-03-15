using System;
using System.Collections.Generic;
using System.Linq;

namespace Vodamep.Aliases
{
    /// <summary>
    /// Kann mit Paaren von Ids befüllt werden .
    /// (Entweder: "Diese beiden Ids entsprechen einer Entity" oder "Diese beiden Ids entsprechen NICHT einer Entity")
    /// Daraus kann dann eine Mappingtabelle der Ids erzeugt werden.
    /// </summary>
    public class AliasSystem
    {
        private readonly AliasSystemConfig _config;
        private readonly HashSet<(string Id1, string Id2)> _notAlias = new HashSet<(string Id1, string Id2)>();
        private readonly Dictionary<string, List<string>> _aliases = new Dictionary<string, List<string>>();        

        public AliasSystem()
            :this(AliasSystemConfig.Default)
        {

        }

        public AliasSystem(AliasSystemConfig config)
        {
            _config = config ?? AliasSystemConfig.Default;

            if (_config.IdRanking == null)
            {
                throw new NullReferenceException("CreateOrderedTuple must not be null!");
            }
        }

        /// <summary>
        /// Diese beiden Ids gehören zu der gleichen Entity
        /// </summary>       
        /// <exception cref="Exception">Wenn ein Wiederspruch gefunden wird, wird eine Exception geworfen</exception>
        public AliasSystem SetAlias(string id1, string id2)
        {
            if (string.IsNullOrEmpty(id1) || string.IsNullOrEmpty(id2))
            {
                return this;
            }

            if (id1.Equals(id2))
            {
                return this;
            }

            var t = _config.IdRanking(id1, id2);

            if (_notAlias.Contains(t))
            {
                throw new Exception($"{t} is already set as not alias!");
            }

            if (_aliases.ContainsKey(t.Second))
            {
                //bisher gab es bereits Einträge für t.Second
                //diese gelten jetzt für t.Main

                _aliases.Add(t.Main, _aliases[t.Second]);
                _aliases.Remove(t.Second);

                _aliases[t.Main].Add(t.Second);
            }
            else if (!_aliases.ContainsKey(t.Main))
            {
                var e1 = _aliases.Where(x => x.Value.Contains(t.Main)).Select(x => x.Key).FirstOrDefault();

                if (e1 != null)
                {
                    return SetAlias(t.Second, e1);
                }

                var e2 = _aliases.Where(x => x.Value.Contains(t.Second)).Select(x => x.Key).FirstOrDefault();

                if (e2 != null)
                {
                    return SetAlias(t.Main, e2);
                }

                _aliases.Add(t.Main, new List<string>() { t.Second });
            }
            else
            {
                if (!_aliases[t.Main].Contains(t.Second))
                {
                    _aliases[t.Main].Add(t.Second);
                }
            }

            return this;
        }

        /// <summary>
        /// Diese beiden Ids gehören NICHT zu der gleichen Entity
        /// </summary>       
        /// <exception cref="Exception">Wenn ein Wiederspruch gefunden wird, wird eine Exception geworfen</exception>
        public AliasSystem SetNotAlias(string id1, string id2)
        {
            if (string.IsNullOrEmpty(id1) || string.IsNullOrEmpty(id2))
            {
                return this;
            }

            if (id1.Equals(id2))
            {
                return this;
            }

            var t = _config.IdRanking(id1, id2);

            if (_aliases.TryGetValue(t.Main, out var list) && list.Contains(t.Second))
            {
                throw new Exception($"{t} is already set as alias!");
            }

            if (!_notAlias.Contains(t))
            {
                _notAlias.Add(t);
            }

            return this;
        }

        public bool IsNotAlias(string id1, string id2) => _notAlias.Contains(_config.IdRanking(id1, id2));

        /// <summary>
        /// Erzeugt die Mapping-Tabelle
        /// </summary>        
        public IDictionary<string, string> BuildMap()
        {
            var result = new SortedDictionary<string, string>();

            foreach (var entry in _aliases)
            {
                foreach (var value in entry.Value)
                {
                    result.Add(value, entry.Key);
                }
            }

            return result;
        }
    }
}
