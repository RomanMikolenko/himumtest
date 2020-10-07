using HiMum.ApiGateway.Domain.Interfaces;
using HiMum.ApiGateway.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiMum.ApiGateway.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly EncryptionServiceConfiguration _configuration;

        public EncryptionService(IHttpClientFactory clientFactory, IOptions<EncryptionServiceConfiguration> options)
        {
            _clientFactory = clientFactory;
            _configuration = options.Value;
        }

        public async Task<ServiceResult> EncryptData(string input, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                using (var content = new JsonContent(new EncryptModel
                {
                    EncryptData = input
                }))
                {
                    var response = await client.PostAsync($"{_configuration.Url}/encrypt", content, cancellationToken);
                    var result = await ReadResponse(response);

                    if (response.IsSuccessStatusCode)
                    {
                        return new ServiceResult
                        {
                            Result = result
                        };
                    }
                    else
                    {
                        return new ServiceResult
                        {
                            Errors = new List<ErrorDetail>
                            {
                                new ErrorDetail
                                {
                                    Message = $"Cannot encrypt input string: {input}. Encryption service responded with unsuccessful code: {response.StatusCode}",
                                    Details = result
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Errors = new List<ErrorDetail>
                    {
                        new ErrorDetail
                        {
                            Message = "Cannot establish connection",
                            Details = ex.Message
                        }
                    }
                };
            }                      
        }

        public async Task<ServiceResult> DecryptData(string input, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                using (var content = new JsonContent(new DecryptModel
                {
                    DecryptData = input
                }))
                {
                    var response = await client.PostAsync($"{_configuration.Url}/decrypt", content, cancellationToken);
                    var result = await ReadResponse(response);

                    if (response.IsSuccessStatusCode)
                    {
                        return new ServiceResult
                        {
                            Result = result
                        };
                    }
                    else
                    {
                        return new ServiceResult
                        {
                            Errors = new List<ErrorDetail>
                            {
                                new ErrorDetail
                                {
                                    Message = $"Cannot decrypt input string: {input}. Encryption service responded with unsuccessful code: {response.StatusCode}",
                                    Details = result
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Errors = new List<ErrorDetail>
                    {
                        new ErrorDetail
                        {
                            Message = "Cannot establish connection",
                            Details = ex.Message
                        }
                    }
                };
            }
        }

        public async Task<ServiceResult> RotateKey(CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync($"{_configuration.Url}/rotate-key", null, cancellationToken);
                var result = await ReadResponse(response);

                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResult();
                }
                else
                {
                    return new ServiceResult
                    {
                        Errors = new List<ErrorDetail>
                        {
                            new ErrorDetail
                            {
                                Message = $"Cannot rotate key. Encryption service responded with unsuccessful code: {response.StatusCode}",
                                Details = result
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Errors = new List<ErrorDetail>
                    {
                        new ErrorDetail
                        {
                            Message = "Cannot establish connection",
                            Details = ex.Message
                        }
                    }
                };
            }
        }

        private static async Task<string> ReadResponse(HttpResponseMessage response)
        {
            var byteArray = await response.Content.ReadAsByteArrayAsync();
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            return responseString;
        }
    }
}
