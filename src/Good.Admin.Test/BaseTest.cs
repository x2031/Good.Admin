﻿using Good.Admin.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Good.Admin.Test
{
    public class BaseTest
    {
        //protected AppConfig AppConfig { get; }
        protected TestServer Server { get; }
        protected HttpClient Client { get; }
        protected IServiceProvider ServiceProvider { get; }
        public BaseTest()
        {
            var application = new WebApplicationFactory<Program>();
            Client = application.CreateClient();
            Server = application.Server;
            ServiceProvider = Server.Services;
        }

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
        public T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
