using HiMum.ApiGateway.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace HiMum.ApiGateway.Domain.Interfaces
{
    public interface IEncryptionService
    {
        Task<ServiceResult> EncryptData(string input, CancellationToken cancellationToken);

        Task<ServiceResult> DecryptData(string input, CancellationToken cancellationToken);

        Task<ServiceResult> RotateKey(CancellationToken cancellationToken);
    }
}
