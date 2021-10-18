using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionsTimeoutsCommon
{
    public static class Deserializer
    {
        // Using System.Text.Json to asynchronously deserialize from Stream
        public static async Task<T> DeserializeRequestBodyAsync<T>(this HttpRequest request)
            => await JsonSerializer.DeserializeAsync<T>(
                request.Body,
                new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
    }
}
