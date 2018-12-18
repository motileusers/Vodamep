using System.Threading;
using System.Threading.Tasks;

namespace Connexia.Service.Client
{
    public interface IValidationClient
    {
        string BaseUrl { get; set; }

        Task<object> ValidateByUserAndInstitutionAsync(string user, string institutionId);
        Task<object> ValidateByUserAndInstitutionAsync(string user, string institutionId, CancellationToken cancellationToken);
    }
}