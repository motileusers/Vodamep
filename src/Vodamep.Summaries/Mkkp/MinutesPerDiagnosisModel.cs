using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public record MinutesPerDiagnosisModel(
        DateTime From,
        DateTime To,
        (string DiagnosisGroups, ActivityScope Scope, int Minues)[] Values
        );

}
