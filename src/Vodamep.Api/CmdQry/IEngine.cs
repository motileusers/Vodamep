using System.Security.Principal;
using Vodamep.ReportBase;

namespace Vodamep.Api.CmdQry
{
    public interface IEngine
    {
        void Login(IPrincipal principal);

        void Execute(ICommand cmd);

        IReport GetPrevious(IReport current);

    }
}
