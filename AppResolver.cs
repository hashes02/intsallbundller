using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace AppBundle.Avalonia
{
    /// <summary>
    /// Resolves application download URLs and metadata from various sources
    /// </summary>
    public static class AppResolver
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly Dictionary<string, ResolvedApp> cache = new Dictionary<string, ResolvedApp>();
        private static readonly TimeSpan cacheExpiry = TimeSpan.FromHours(6);

        static AppResolver()
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        /// <summary>
        /// Resolves download URL for an application, using cache when available
        /// </summary>
        public static async Task<ResolvedApp?> ResolveAppAsync(AppInfo app)
        {
            // Check cache first
            if (cache.ContainsKey(app.Id))
            {
                var cached = cache[app.Id];
                if (DateTime.Now - cached.CacheTime < cacheExpiry)
                {
                    return cached;
                }
            }

            ResolvedApp? resolved = null;

            try
            {
                resolved = app.Source.ToLower() switch
                {
                    "omaha" => await ResolveOmahaAsync(app),
                    "videolan" => await ResolveVideolanAsync(app),
                    "direct" => ResolveDirect(app),
                    _ => null
                };

                if (resolved != null)
                {
                    cache[app.Id] = resolved;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving {app.Name}: {ex.Message}");
            }

            return resolved;
        }

        /// <summary>
        /// Resolves Chrome download URL using Omaha update protocol
        /// </summary>
        private static async Task<ResolvedApp?> ResolveOmahaAsync(AppInfo app)
        {
            var downloadUrl = "https://dl.google.com/chrome/install/ChromeStandaloneSetup64.exe";
            
            try
            {
                var finalUrl = await FollowRedirectAsync(downloadUrl);
                
                return new ResolvedApp
                {
                    Url = finalUrl,
                    Version = "Latest",
                    Sha256 = null // Skip hash computation for large files
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chrome resolver error: {ex.Message}");
                
                return new ResolvedApp
                {
                    Url = downloadUrl,
                    Version = "Latest",
                    Sha256 = null
                };
            }
        }

        /// <summary>
        /// Follows HTTP redirects to get the final URL
        /// </summary>
        private static async Task<string> FollowRedirectAsync(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            using var response = await httpClient.SendAsync(request);
            return response.RequestMessage?.RequestUri?.AbsoluteUri ?? url;
        }

        /// <summary>
        /// Computes SHA256 hash from a file path
        /// </summary>
        private static async Task<string> ComputeHashAsync(string url)
        {
            using var response = await httpClient.GetAsync(url);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(stream);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// Resolves VLC download URL from VideoLAN website
        /// </summary>
        private static async Task<ResolvedApp?> ResolveVideolanAsync(AppInfo app)
        {
            try
            {
                var response = await httpClient.GetStringAsync("https://get.videolan.org/vlc/last/win64/");
                
                var exePattern = @"href=""([^""]*\.exe)""";
                var sha256Pattern = @"href=""([^""]*\.sha256)""";
                
                var exeMatch = Regex.Match(response, exePattern);
                var sha256Match = Regex.Match(response, sha256Pattern);
                
                if (exeMatch.Success)
                {
                    var exeFile = exeMatch.Groups[1].Value;
                    var url = $"https://get.videolan.org/vlc/last/win64/{exeFile}";
                    
                    string? sha256 = null;
                    if (sha256Match.Success)
                    {
                        try
                        {
                            var sha256File = sha256Match.Groups[1].Value;
                            var sha256Url = $"https://get.videolan.org/vlc/last/win64/{sha256File}";
                            var sha256Response = await httpClient.GetStringAsync(sha256Url);
                            sha256 = sha256Response.Split(' ')[0].Trim();
                        }
                        catch
                        {
                            // SHA256 fetch is optional
                        }
                    }
                    
                    return new ResolvedApp
                    {
                        Url = url,
                        Sha256 = sha256
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving VLC via VideoLAN: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Returns direct URL from app configuration
        /// </summary>
        private static ResolvedApp? ResolveDirect(AppInfo app)
        {
            // Direct URL - use as provided in JSON
            if (!string.IsNullOrEmpty(app.Url))
            {
                return new ResolvedApp
                {
                    Url = app.Url,
                    Sha256 = app.Sha256
                };
            }

            return null;
        }

        /// <summary>
        /// Computes SHA256 hash of a file
        /// </summary>
        public static async Task<string?> ComputeSha256Async(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                using var sha256 = SHA256.Create();
                using var stream = File.OpenRead(filePath);
                var hash = await sha256.ComputeHashAsync(stream);
                return Convert.ToHexString(hash).ToLowerInvariant();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error computing SHA256 for {filePath}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Verifies file hash against expected SHA256
        /// </summary>
        public static async Task<bool> VerifySha256Async(string filePath, string? expectedSha256)
        {
            if (string.IsNullOrWhiteSpace(expectedSha256))
                return true;

            var actualSha256 = await ComputeSha256Async(filePath);
            if (actualSha256 == null)
                return false;

            return string.Equals(actualSha256, expectedSha256.Replace("sha256:", "", StringComparison.OrdinalIgnoreCase), 
                StringComparison.OrdinalIgnoreCase);
        }
    }
}