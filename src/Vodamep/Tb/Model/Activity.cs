using Vodamep.ReportBase;

namespace Vodamep.Tb.Model
{
    public partial class Activity : IPersonActivity
    {
        public float Time => this.HoursPerMonth;
    }
}