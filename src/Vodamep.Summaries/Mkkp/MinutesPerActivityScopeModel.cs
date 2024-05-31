using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public record MinutesPerActivityScopeModel(
        DateTime From, 
        DateTime To, 
        (string Id, string Name)[] Names, 
        (string Id, ActivityScope Scope, int Minues)[] Values
        );

}
