using PowerArgs;

namespace Vodamep.Client
{
    public class ValidatePreviousArgs : ArgsBase
    {        
        [ArgRequired, ArgPosition(1)]
        public string File { get; set; }

        [ArgRequired, ArgPosition(2)]
        public string PreviousFile { get; set; }

        public bool IgnoreWarnings { get; set; } = false;
    }
}
