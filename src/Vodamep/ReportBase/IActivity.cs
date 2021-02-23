using System;
using System.Collections.Generic;

namespace Vodamep.ReportBase
{
    public interface IActivity<T> where T : Enum
    {
        DateTime DateD { get; }
        string PersonId { get; }
        string StaffId { get; }

        IEnumerable<T> EntriesT { get; }
    }
}