using PowerArgs;

namespace Vodamep.Client
{
    public class SendArgs : ArgsBase
    {
        [ArgRequired, ArgPosition(1)]
        public string File { get; set; }

        [ArgRequired]
        public string Address { get; set; }

        public string User { get; set; }
                
        public string Password { get; set; }
    }
}
