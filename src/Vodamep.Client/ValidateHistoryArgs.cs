using PowerArgs;

namespace Vodamep.Client
{
    public class ValidateHistoryArgs : ArgsBase
    {
        [ArgRequired, ArgPosition(1)]
        public string File { get; set; }

        [ArgRequired, ArgPosition(2)]
        public string HistoryFiles { get; set; }
        
        public bool IgnoreWarnings { get; set; } = false;
    }
}
