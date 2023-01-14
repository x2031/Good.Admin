using Good.Admin.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Good.Admin.Test
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testserver;
        public HttpClient Client { get; }
        public TestServerFixture()
        {
            var builder = new WebApplicationFactory<Program>();
            _testserver = builder.Server;
            Client = _testserver.CreateClient();
        }
        public T GetService<T>()
        {
            return _testserver.Services.GetService<T>();
        }
        public T GetRequiredService<T>()
        {
            return _testserver.Services.GetRequiredService<T>();
        }
        public void Dispose()
        {
            Client.Dispose();
            _testserver.Dispose();
        }

    }
}
