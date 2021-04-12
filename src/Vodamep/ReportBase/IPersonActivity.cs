using System;

namespace Vodamep.ReportBase
{
    public interface IPersonActivity : IPersonId
    {
        float Time { get; }
    }
}
