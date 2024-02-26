using System.Text.Json.Serialization;

namespace SmallsOnline.VSCode.Configurator;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never
)]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(int[]))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(long[]))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(double[]))]
internal partial class CoreJsonContext : JsonSerializerContext
{}