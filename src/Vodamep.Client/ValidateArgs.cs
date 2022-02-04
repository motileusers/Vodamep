using PowerArgs;

namespace Vodamep.Client
{
    public class ValidateArgs : ArgsBase
    {        
        [ArgRequired, ArgPosition(1)]
        [ArgExample("validate -f current.json", "")]
        [ArgDescription("Meldung, die validiert werden soll")]
        public string File { get; set; }

        [ArgPosition(2)]
        [ArgExample("validate -f current.json -p previous.json", "")]
        [ArgDescription("Vorgänger Meldung (vom vorherigen Zeitraum), gegen die validiert wird")]
        public string PreviousFile { get; set; }

        [ArgPosition(3)]
        [ArgDescription("Bereits vorhandene Meldung (vom gleichen Zeitraum), gegen die validiert wird")]
        [ArgExample("validate -f current.json -e existing.json", "")]
        public string ExistingFile { get; set; }


        public bool IgnoreWarnings { get; set; } = false;
    }
}
