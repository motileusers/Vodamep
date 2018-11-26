using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodamep.Legacy.Model
{
    public class AnstellungDTO
    {
        public Nullable<DateTime> Von { get; set; }

        public Nullable<DateTime> Bis { get; set; }

        public float VZAE { get; set; }

        public int Pflegernummer { get; set; }

        public string Berufstitel { get; set; }

    }
}
