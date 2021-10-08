using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp3
{
    public static class Function3
    {
        [FunctionName("Function3")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Multiple cancellation tokens can be linked so that either can request to cancel
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                req.HttpContext.RequestAborted))
            {
                for (int i = 1; i <= 15; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), linkedTokenSource.Token);
                    if (linkedTokenSource.Token.IsCancellationRequested) throw new Exception($"Request aborted after {i} seconds");
                }

                return new OkObjectResult("Hello from Function 3");
            }
        }
    }
}
