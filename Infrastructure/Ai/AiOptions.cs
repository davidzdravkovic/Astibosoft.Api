namespace Astibosoft.Api.Infrastructure.Ai;

public class AiOptions
{
    public const string SectionName = "Ai";

    /// <summary>Base URL of Astibosoft.Ai (e.g. http://localhost:8090).</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Shared secret sent as X-Internal-Key (same as RAG_INTERNAL_KEY on Python).</summary>
    public string InternalKey { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 60;
}
