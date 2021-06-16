using System;

namespace Vodamep.ReportBase
{
    public interface IActivity
    {
        DateTime DateD { get; }
        string PersonId { get; }
        string StaffId { get; }
    }
}