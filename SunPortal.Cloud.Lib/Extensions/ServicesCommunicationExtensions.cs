using System.Text.Json;
using System.Text.Json.Serialization;
using SunPortal.Cloud.Lib.Models;

namespace SunPortal.Cloud.Lib.Extensions;

public static class ServicesCommunicationExtensions
{
    public static byte[] ToMessage(this Object request)
    {
        return JsonSerializer.SerializeToUtf8Bytes(request);
    }

    public static T? ToObject<T>(this byte[] response)
    {
        return JsonSerializer.Deserialize<T>(response);
    }
}