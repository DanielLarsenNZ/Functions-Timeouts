using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp2
{
    public static class Function2
    {
        private static readonly HttpClient _http = new HttpClient() { Timeout = TimeSpan.FromSeconds(100) };

        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var response = await _http.GetAsync("http://localhost:7073/api/Function3", cancellationToken);
            var content = await response.Content.ReadAsAsync<string>(cancellationToken);

            if (!response.IsSuccessStatusCode) return new ObjectResult(content) { StatusCode = (int)response.StatusCode };
            return new OkObjectResult(content);
        }
    }
}
