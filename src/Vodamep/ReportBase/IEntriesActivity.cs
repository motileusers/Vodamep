using System;
using System.Collections.Generic;

namespace Vodamep.ReportBase
{
    public interface IEntriesActivity<T> : IActivity where T : Enum
    {
        IEnumerable<T> EntriesT { get; }
    }
}