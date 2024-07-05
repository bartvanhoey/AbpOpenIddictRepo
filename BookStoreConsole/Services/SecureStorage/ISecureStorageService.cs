using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreConsole.Services.SecureStorage
{
    public interface ISecureStorageService
    {
        Task SetAccessTokenAsync(string accessToken);
         Task<string> GetAccessTokenAsync();
    }

    public class SecureStorageService : ISecureStorageService
    {
        public async Task<string> GetAccessTokenAsync()
        {
            await Task.CompletedTask;
            return string.Empty;
        }

        public async Task SetAccessTokenAsync(string accessToken)
        {
            await Task.CompletedTask;
        }
    }
}