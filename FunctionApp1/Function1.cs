using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class Function1
    {
        private static readonly HttpClient _http = new HttpClient() { Timeout = TimeSpan.FromSeconds(100) };

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // create a list of ids to pass in the request body
            const int totalItems = 200;
            var listOfIds = new List<string>(totalItems);
            for (int i = 0; i < totalItems; i++) listOfIds.Add($"{i + 1}");
            var content = JsonContent.Create(listOfIds.ToArray());

            string url = "http://localhost:7072/api/Function2";

            // Multiple cancellation tokens can be linked so that either can request to cancel
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                req.HttpContext.RequestAborted))
            {
                var response = await _http.PostAsync(url, content, linkedTokenSource.Token);
                if (!response.IsSuccessStatusCode) return new ObjectResult(response.ReasonPhrase) { StatusCode = (int)response.StatusCode };

                var items = await response.Content.ReadAsAsync<string[]>(cancellationToken);
                return new OkObjectResult(items);
            }
        }
    }
}
