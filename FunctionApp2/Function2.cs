using FunctionsTimeoutsCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp2
{
    public static class Function2
    {
        private static readonly HttpClient _http = new HttpClient() { Timeout = TimeSpan.FromSeconds(100) };

        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // get ids from request body. See extension method in FunctionsTimeoutsCommon project
            string[] ids = await request.DeserializeRequestBodyAsync<string[]>();
            
            // Aggregate to a distinct list of ids to send in request body
            string[] distinctIds = ids.ToList().Distinct().ToArray();
            var content = JsonContent.Create(distinctIds);

            string url = "http://localhost:7073/api/Function3";

            // Multiple cancellation tokens can be linked so that either can request to cancel
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                request.HttpContext.RequestAborted))
            {
                var response = await _http.PostAsync(url, content, linkedTokenSource.Token);
                if (!response.IsSuccessStatusCode) return new ObjectResult(response.ReasonPhrase) { StatusCode = (int)response.StatusCode };

                var items = await response.Content.ReadAsAsync<IEnumerable<string>>(cancellationToken);
                return new OkObjectResult(items.Distinct());
            }
        }
    }
}
