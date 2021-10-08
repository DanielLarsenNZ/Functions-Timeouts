using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class Function1
    {
        private static readonly HttpClient _http = new HttpClient() { Timeout = TimeSpan.FromSeconds(100) };

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string url = "http://localhost:7072/api/Function2";

            // Multiple cancellation tokens can be linked so that either can request to cancel
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                req.HttpContext.RequestAborted))
            {
                var response = await _http.GetAsync(url, linkedTokenSource.Token);
                var content = await response.Content.ReadAsStringAsync();   // only good for short responses. Streaming responses should use a method that accepts a cancellation token.
                if (!response.IsSuccessStatusCode) return new ObjectResult(string.IsNullOrWhiteSpace(content) ? response.ReasonPhrase : content) { StatusCode = (int)response.StatusCode };
                return new OkObjectResult(content);
            }
        }
    }
}
