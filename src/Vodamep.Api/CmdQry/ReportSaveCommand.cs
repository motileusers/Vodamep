using Vodamep.ReportBase;

namespace Vodamep.Api.CmdQry
{
    public class ReportSaveCommand : ICommand
    {
        public IReportBase Report { get; set; }
    }
}
