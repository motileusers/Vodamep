using Vodamep.ReportBase;

namespace Vodamep.Mohi.Model
{
    public partial class Activity : IPersonActivity
    {
        public float Time => this.HoursPerMonth;
    }
}