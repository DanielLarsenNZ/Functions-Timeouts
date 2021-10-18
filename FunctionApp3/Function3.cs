using FunctionsTimeoutsCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp3
{
    public static class Function3
    {
        [FunctionName("Function3")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // get ids from request body. See extension method in FunctionsTimeoutsCommon project
            string[] ids = await request.DeserializeRequestBodyAsync<string[]>();

            // Multiple cancellation tokens can be linked so that either can request to cancel
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                request.HttpContext.RequestAborted))
            {
                int millisecondsToDelay = 50;
                var items = new List<string>(ids.Length);

                // Simulating slow cancellable IO in this loop
                var stopwatch = Stopwatch.StartNew();
                foreach (string id in ids)
                {
                    await Task.Delay(millisecondsToDelay, linkedTokenSource.Token); // simulating IO delay
                    if (linkedTokenSource.Token.IsCancellationRequested) throw new Exception($"Request aborted after ~{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} seconds");
                    items.Add($"Hello from Function 3 - id = {id}");
                }

                return new OkObjectResult(items);
            }
        }
    }
}
