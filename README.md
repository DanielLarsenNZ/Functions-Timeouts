# Handling Timeouts in Functions



## Getting Started

    PS> ./run.ps1

## Questions

How does DAPR handle timeouts (and GRPC)

## References

<https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json>

<https://johnthiriet.com/efficient-post-calls/>

<https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices?view=aspnetcore-5.0#avoid-blocking-calls>

https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1#return-ienumerablet-or-iasyncenumerablet

Release It! 2nd edition by Michael T. Nygard. Timeouts: <https://learning.oreilly.com/library/view/release-it-2nd/9781680504552/f_0048.xhtml#stability-patterns.timeout>

`HttpContext.RequestAborted` <https://stackoverflow.com/a/51816341/610731>

Listening to Multiple Tokens Simultaneously: <https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads#listening-to-multiple-tokens-simultaneously>

## Research links

GRPC in .NET <https://docs.microsoft.com/en-us/aspnet/core/grpc/basics?view=aspnetcore-5.0>

Cancellation in GRPC <https://docs.microsoft.com/en-us/aspnet/core/grpc/deadlines-cancellation?view=aspnetcore-5.0#cancellation>

Azure Functions Language Worker Protobuf: <https://github.com/Azure/azure-functions-language-worker-protobuf>

HTTP2 does not solve HTTP request cancellation <https://developers.google.com/web/fundamentals/performance/http2#push_promise_101>

<https://stackoverflow.com/questions/2652082/can-a-http-server-detect-that-a-client-has-cancelled-their-request>

`Microsoft.Azure.Functions.Worker.Grpc` <https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Grpc/>
