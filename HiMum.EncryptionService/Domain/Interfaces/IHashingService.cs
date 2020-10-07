using System.Threading.Tasks;

namespace HiMum.EncryptionService.Domain.Interfaces
{
    public interface IHashingService
    {
        void GenerateSalt();

        Task<string> EncryptData(string input);

        Task<string> DecryptData(string input);
    }
}
