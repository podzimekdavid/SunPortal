using System.Text.Json;
using System.Text.Json.Serialization;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Communication.Parameters;

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

    public const string ERROR_CONVERT_STRING = "error";
    public static string ToValueText(this byte[] data, ParameterType type)
    {
        try
        {
            switch (type)
            {
                case ParameterType.Float:
                    return ToFloat(data).ToString();
                case ParameterType.Int:
                    throw new NotImplementedException();
                case ParameterType.Bool:
                    return ToBool(data).ToString();
                case ParameterType.ShortEnum:
                    return ToShortEnum(data).ToString();
                case ParameterType.LongEnum:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        catch (Exception)
        {
            return ERROR_CONVERT_STRING;
        }

    }

    public static float ToFloat(this byte[] data)
    {
        return BitConverter.ToSingle(data, 0);
    }

    public static int ToShortEnum(this byte[] data)
    {
        return BitConverter.ToUInt16(data, 0);
    }

    public static bool ToBool(this byte[] data)
    {
        return BitConverter.ToBoolean(data, 0);
    }
}