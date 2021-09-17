using Vodamep.ReportBase;

namespace Vodamep.Api.CmdQry
{
    public class ReportSaveCommand : ICommand
    {
        public IReport Report { get; set; }
    }
}
