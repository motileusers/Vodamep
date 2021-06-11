using System;

namespace Vodamep.ReportBase
{
    public interface IMinutesActivity
    {
        string PersonId { get; }
        int Minutes { get; }
    }
}