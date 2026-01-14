using System;
using System.IO;
using System.Linq;

namespace Unsplash.Automation.Tests.Utils;

public static class DownloadHelper
{
    // DownloadHelper: utility methods for snapshotting and waiting for files in the user's
    // Downloads folder. Kept out of PageObjects so filesystem concerns stay in Utils.
    public static string GetDownloadsPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    }

    public static string[] SnapshotDownloads(string? downloadsPath = null)
    {
        var path = downloadsPath ?? GetDownloadsPath();
        try { return Directory.GetFiles(path); } catch { return Array.Empty<string>(); }
    }

    public static string? WaitForNewImage(string[] beforeFiles, int timeoutSeconds = 20, string? downloadsPath = null)
    {
        var path = downloadsPath ?? GetDownloadsPath();
        string? downloadedFile = null;
        var attempts = Math.Max(1, timeoutSeconds);
        for (int i = 0; i < attempts; i++)
        {
            TestConfig.Pause(1000);
            string[] currentFiles;
            try { currentFiles = Directory.GetFiles(path); } catch { currentFiles = Array.Empty<string>(); }
            var newFiles = currentFiles.Except(beforeFiles).ToList();

            if (newFiles.Any())
            {
                if (newFiles.Any(f => f.EndsWith(".crdownload") || f.EndsWith(".tmp")))
                {
                    continue;
                }

                downloadedFile = newFiles.FirstOrDefault(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"));
                if (downloadedFile != null) break;
            }
        }

        return downloadedFile;
    }

    public static void DeleteFile(string? path)
    {
        if (string.IsNullOrEmpty(path)) return;
        try { if (File.Exists(path)) File.Delete(path); } catch (Exception ex) { Logger.Warn($"Failed to delete {path}: {ex.Message}"); }
    }
}
