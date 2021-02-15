using PowerArgs;

namespace Vodamep.Client
{
    public abstract class PackRandomArgs
    {
        [ArgDescription("Save as JSON.")]
        public bool Json { get; set; } = false;

        [DefaultValue(false)]
        public bool NoCompression { get; set; }
    }
}