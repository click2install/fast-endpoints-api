using System.Net;
using System.Net.Http.Json;

using Microsoft.Net.Http.Headers;

namespace FastEndApi.E2E.Tests.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static async Task<(HttpStatusCode statusCode, T data)> Extract<T>(this HttpResponseMessage response)
    {
        var body = await response.Content.ReadFromJsonAsync<T>();

        return (response.StatusCode, body)!;
    }
}
