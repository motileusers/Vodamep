using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public partial class ClientActivity : IPersonDateActivity
    {
        public float Time => this.Minutes;
    }
}