namespace Shared.Options;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
}
