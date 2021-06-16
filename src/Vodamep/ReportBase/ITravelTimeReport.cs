using System.Collections.Generic;

namespace Vodamep.ReportBase
{
    public interface ITravelTimeReport : IReport
    {
        IList<IStaff> Staffs { get; }

        IList<ITravelTime> TravelTimes { get; }
    }
}