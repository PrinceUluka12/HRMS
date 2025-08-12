namespace HRMS.Infrastructure.Persistence.Helpers;

public static class JsonHelper
{
    public static string SerializeStringArray(string[] array)
        => System.Text.Json.JsonSerializer.Serialize(array);

    public static string[] DeserializeStringArray(string json)
        => string.IsNullOrEmpty(json)
            ? Array.Empty<string>()
            : System.Text.Json.JsonSerializer.Deserialize<string[]>(json);
}