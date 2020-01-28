using PowerArgs;

namespace Vodamep.Client
{
    public class DiffArgs
    {
        [ArgRequired, ArgShortcut("F1"), ArgPosition(1)]
        public string File1 { get; set; }

        [ArgRequired, ArgShortcut("F2"), ArgPosition(2)]
        public string File2 { get; set; }

        [ArgRequired, ArgShortcut("FO"), ArgPosition(3)]
        public string FileOutput { get; set; }
    }
}