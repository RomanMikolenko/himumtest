using HiMum.EncryptionService.Domain.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HiMum.EncryptionService.Infrastructure.Services
{
    public class HashingService : IHashingService
    {
        private byte[] _salt;

        public HashingService()
        {
            GenerateSalt();
        }

        public async Task<string> DecryptData(string input)
        {
            var encodedInput = Convert.FromBase64String(input);

            var iv = new byte[16];
            var cipher = new byte[encodedInput.Length - iv.Length];

            Buffer.BlockCopy(encodedInput, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encodedInput, iv.Length, cipher, 0, encodedInput.Length - iv.Length);

            try
            {
                using (var algorithm = Aes.Create())
                {
                    using (var decryptor = algorithm.CreateDecryptor(_salt, iv))
                    {
                        using (var memoryStream = new MemoryStream(cipher))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (var streamReader = new StreamReader(cryptoStream))
                                {
                                    return await streamReader.ReadToEndAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Input data is invalid", ex);
            }            
        }

        public async Task<string> EncryptData(string input)
        {
            using (var algorithm = Aes.Create())
            {
                using (var encryptor = algorithm.CreateEncryptor(_salt, algorithm.IV))
                { 
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                await streamWriter.WriteAsync(input);
                            }

                            var iv = algorithm.IV;
                            var decryptedContent = memoryStream.ToArray();
                            var result = new byte[iv.Length + decryptedContent.Length];
                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
        }

        public void GenerateSalt()
        {
            _salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(_salt);
            }
        }
    }
}
