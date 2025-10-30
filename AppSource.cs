using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppBundle.Avalonia
{
    /// <summary>
    /// Installation status for an application
    /// </summary>
    public enum InstallStatus
    {
        NotStarted,
        Downloading,
        Installing,
        Done,
        Failed,
        Skipped
    }

    /// <summary>
    /// Application information from apps.json
    /// </summary>
    public class AppInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("arch")]
        public string? Arch { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("args")]
        public string? Args { get; set; }

        [JsonPropertyName("detect")]
        public string? Detect { get; set; }

        [JsonPropertyName("sha256")]
        public string? Sha256 { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; } = true;

        [JsonIgnore]
        public InstallStatus Status { get; set; } = InstallStatus.NotStarted;

        [JsonIgnore]
        public string StatusText { get; set; } = string.Empty;

        [JsonIgnore]
        public string? ResolvedUrl { get; set; }

        [JsonIgnore]
        public string? ResolvedSha256 { get; set; }

        [JsonIgnore]
        public DateTime? LastCacheUpdate { get; set; }
    }

    /// <summary>
    /// Root object for apps.json
    /// </summary>
    public class AppCollection
    {
        [JsonPropertyName("apps")]
        public List<AppInfo> Apps { get; set; } = new List<AppInfo>();
    }

    /// <summary>
    /// Resolved application metadata from various sources
    /// </summary>
    public class ResolvedApp
    {
        public string Url { get; set; } = string.Empty;
        public string? Sha256 { get; set; }
        public string? Version { get; set; }
        public DateTime CacheTime { get; set; } = DateTime.Now;
    }
}