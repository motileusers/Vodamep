using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public record MinutesPerDiagnosisModel(
        DateTime From, 
        DateTime To, 
        (string Id, DiagnosisGroup[] Diagnosis)[] Diagnosis, 
        (string Id, ActivityScope Scope, int Minues)[] Values
        );

}
