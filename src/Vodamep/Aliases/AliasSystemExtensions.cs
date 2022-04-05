using System;
using System.Collections.Generic;

namespace Vodamep.Aliases
{
    public static class AliasSystemExtensions
    {       
        /// <summary>
        /// Dubletten suchen, indem das Entity zu einem String reduziert wird. 
        /// Es soll immer das gesammte Set übergeben werden, dieser Aufruf sollt nur einmalig erfolgen.
        /// </summary>       
        /// <param name="getId">So erhält man den Id des Entities.</param>
        /// <param name="reduce">So wird das Entity zu einem String reduziert.</param>        
        public static AliasSystem SetAliases<T>(this AliasSystem system, IEnumerable<T> entries, Func<T, string> getId, Func<T, string> reduce)
        {
            var s = new SortedDictionary<string, T>();
            foreach (var entry in entries)
            {
                var reduced = reduce(entry);

                if (s.TryGetValue(reduced, out T v))
                {
                    var id1 = getId(entry);
                    var id2 = getId(v);

                    if (!system.IsNotAlias(id1, id2))
                    {
                        system.SetAlias(id1, id2);
                    }
                }
                else
                {
                    s.Add(reduced, entry);
                }
            }

            return system;
        }

        public static AliasSystem SetAliases(this AliasSystem system, IEnumerable<(string Id1, string Id2)> ids)
        {
            if (ids != null)
            {
                foreach (var entry in ids)
                {
                    system.SetAlias(entry.Id1, entry.Id2);
                }
            }
            return system;
        }

        public static AliasSystem SetNotAliases(this AliasSystem system, IEnumerable<(string Id1, string Id2)> ids)
        {
            if (ids != null)
            {
                foreach (var entry in ids)
                {
                    system.SetNotAlias(entry.Id1, entry.Id2);
                }
            }
            return system;
        }


    }
}
