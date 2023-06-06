using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Api.Extensions;

public static class StreamExtensions
{
    public static async Task<T> ConvertFrom<T>(this Stream body, JsonSerializerOptions jsonOptions, ILogger logger)
    {
        using (logger.BeginScope("StreamExtensions.ConvertFrom")) {
            logger.LogInformation("Converting stream to object");
            var requestBody = await new StreamReader(body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody)){
                logger.LogError("Request body is empty");
                throw new ArgumentNullException(nameof(body));
            }
                
            return JsonSerializer.Deserialize<T>(requestBody, jsonOptions);
        }
    }
}