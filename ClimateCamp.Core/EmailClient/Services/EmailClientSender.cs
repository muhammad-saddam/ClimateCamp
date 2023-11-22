using Abp.Dependency;
using ClimateCamp.Core.EmailClient;
using ClimateCamp.EmailClient.Helpers;
using ClimateCamp.EmailClient.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClimateCamp.EmailClient.Services
{
    public class EmailClientSender : IEmailClientSender, ISingletonDependency
    {
        private readonly IMemoryCache memoryCache;
        private readonly IHttpClientFactory _clientFactory;

        public EmailClientSender(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            this.memoryCache = memoryCache;
        }

        public async Task<string> SendEmail(EmailSenderModel payload, string EmailSenderServiceUrl)
        {
            if (payload.TemplateName != string.Empty)
            {
                RazorParser renderer = new RazorParser(typeof(EmailSenderClient).Assembly, memoryCache);
                var body = renderer.UsingTemplateFromEmbedded($"EmailClient.Templates.{payload.TemplateName}", payload);
                payload.Body = body;
            }

            var client = _clientFactory.CreateClient();
            string jsonString = JsonSerializer.Serialize(payload);
            var finalPayload = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(EmailSenderServiceUrl, finalPayload);
            string responseJson = await response.Content.ReadAsStringAsync();
            return responseJson;
        }
    }
}
